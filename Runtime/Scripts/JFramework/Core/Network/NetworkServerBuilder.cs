namespace JFramework
{
    public class NetworkServerBuilder
    {
        IJSocketListener socket;

        ISocketListenerFactory socketFactory;

        INetworkMessageProcessStrate networkMessageProcessStrate;

        INetworkServerMessageHandler networkMessageHandler;

        INetMessageSerializerStrate netMessageSerializerStrate;

        IMessageTypeResolver messageTypeResolver;

        JDataProcesserManager outProcesserManager;

        JDataProcesserManager comingProcesserManager;

        IDataConverter dataConverter;

        ITypeRegister protocolRegister;

        ITokenManager tokenManager;

        public IJNetworkServer Build()
        {
            if (socket == null)
            {
                socket = new JSocketListener();
            }

            if (socketFactory == null)
            {
                socketFactory = new JSocketListenerFactory(socket);
            }

            if (dataConverter == null)
            {
                dataConverter = new JDataConverter();
            }

            if (netMessageSerializerStrate == null)
            {
                netMessageSerializerStrate = new JNetMessageJsonSerializerStrate(dataConverter);
            }

            if (protocolRegister == null)
            {
                throw new System.Exception("Protocol register is required. Please set it using SetProtocolRegister method.");
            }

            if (messageTypeResolver == null)
            {
                messageTypeResolver = new JNetMessageJsonTypeResolver(dataConverter, protocolRegister);
            }

            if (outProcesserManager == null)
            {
                //outProcesserManager = new JDataProcesserManager();
            }

            if (comingProcesserManager == null)
            {
                //comingProcesserManager = new JDataProcesserManager();
            }

            if (networkMessageProcessStrate == null)
            {
                networkMessageProcessStrate = new JNetworkMessageProcessStrate(netMessageSerializerStrate, messageTypeResolver, outProcesserManager, comingProcesserManager);
            }

            if(networkMessageHandler == null)
            {
                throw new System.Exception("Network message handler is required. Please set it using SetMessageHandler method.");
            }

            if(tokenManager == null)
            {
                tokenManager = new JTokenManager();
            }

            var network = new JNetworkServer(socketFactory, networkMessageProcessStrate, networkMessageHandler, tokenManager);
            return network;
        }

        public NetworkServerBuilder SetSocketFactory(ISocketListenerFactory factory)
        {
            socketFactory = factory;
            return this;
        }



        public NetworkServerBuilder SetMessageProcessStrate(INetworkMessageProcessStrate strate)
        {
            networkMessageProcessStrate = strate;
            return this;
        }

        public NetworkServerBuilder SetMessageHandler(INetworkServerMessageHandler handler)
        {
            networkMessageHandler = handler;
            return this;
        }

        public NetworkServerBuilder SetSocket(IJSocketListener socket)
        {
            this.socket = socket;
            return this;
        }

        public NetworkServerBuilder SetNetMessageSerializerStrate(INetMessageSerializerStrate strate)
        {
            netMessageSerializerStrate = strate;
            return this;
        }

        public NetworkServerBuilder SetMessageTypeResolver(IMessageTypeResolver resolver)
        {
            messageTypeResolver = resolver;
            return this;
        }

        public NetworkServerBuilder SetOutDataProcesser(JDataProcesserManager processer)
        {
            outProcesserManager = processer;
            return this;
        }

        public NetworkServerBuilder SetComingDataProcesser(JDataProcesserManager processer)
        {
            comingProcesserManager = processer;
            return this;
        }

        public NetworkServerBuilder SetDataConverter(IDataConverter converter)
        {
            dataConverter = converter;
            return this;
        }

        public NetworkServerBuilder SetProtocolRegister(ITypeRegister register)
        {
            protocolRegister = register;
            return this;
        }
    }
}