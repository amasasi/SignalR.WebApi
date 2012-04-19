using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Hosting;

namespace SignalR.AspNetWebApi
{
    internal class WebApiResponse : IResponse
    {
        private readonly CancellationToken cancellationToken;
        private readonly HttpResponseMessage responseMessage;
        private Stream streamingStream;
        private readonly Action triggerResponse;
        private int streamingInitialized;

        public WebApiResponse(CancellationToken cancellationToken, HttpResponseMessage responseMessage, Action triggerResponse)
        {
            this.cancellationToken = cancellationToken;
            this.triggerResponse = triggerResponse;
            this.responseMessage = responseMessage;
        }

        public string ContentType { get; set; }

        public HttpResponseMessage ResponseMessage
        {
            get
            {
                return responseMessage;
            }
        }

        public bool IsClientConnected
        {
            get
            {
                // TODO: check if writes fail - which isn't a too bad check
                return true; // cancellationToken.IsCancellationRequested;
            }
        }

        public Task EndAsync(string data)
        {
            responseMessage.Content = new StringContent(data);
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);

            return TaskAsyncHelper.Empty;
        }

        public Task WriteAsync(string data)
        {
            if (Interlocked.Exchange(ref streamingInitialized, 1) == 0)
            {
                responseMessage.Headers.TransferEncodingChunked = true;
                responseMessage.Content = new ActionOfStreamContent(stream =>
                {
                    streamingStream = stream;
                });

                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);

                triggerResponse();
            }

            return WriteTaskAsync(data).Catch();
        }

        private Task WriteTaskAsync(string data)
        {
            if (streamingStream == null)
            {
                return TaskAsyncHelper.Empty;
            }

            var buffer = Encoding.UTF8.GetBytes(data);

            return Task.Factory.FromAsync(
                (cb, state) => streamingStream.BeginWrite(buffer, 0, buffer.Length, cb, state), ar => streamingStream.EndWrite(ar), null)
                .Then(() => streamingStream.Flush());
        }

        public class ActionOfStreamContent : HttpContent
        {
            private readonly Action<Stream> actionOfStream;

            public ActionOfStreamContent(Action<Stream> actionOfStream)
            {
                if (actionOfStream == null)
                {
                    throw new ArgumentNullException("actionOfStream");
                }

                this.actionOfStream = actionOfStream;
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return Task.Factory.StartNew(
                    (obj) =>
                    {
                        var target = obj as Stream;
                        actionOfStream(target);
                    },
                    stream);
            }

            protected override bool TryComputeLength(out long length)
            {
                length = -1;

                return false;
            }
        }
    }
}
