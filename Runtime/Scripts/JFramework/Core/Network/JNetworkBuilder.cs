//using System.Reactive.Linq;
//using System.Reactive.Subjects;
using System.Net.Sockets;

namespace JFramework
{
    public class JNetworkBuilder
    {
        IJSocket socket;

        ISocketFactory socketFactory;

        IJTaskCompletionSourceManager<IUnique> taskManager;

        INetworkMessageProcessStrate networkMessageProcessStrate;

        INetworkMessageHandler networkMessageHandler;

        INetMessageSerializerStrate netMessageSerializerStrate;

        IMessageTypeResolver messageTypeResolver;

        JDataProcesserManager outProcesserManager;

        JDataProcesserManager comingProcesserManager;

        IDataConverter dataConverter;

        ITypeRegister protocolRegister;

        bool useNetworkTaskManager = false;

        public IJNetwork Build()
        {
            if (socket == null)
            {
                socket = new JSocket();
            }

            if (socketFactory == null)
            {
                socketFactory = new JSocketFactory(socket);
            }

            if (taskManager == null && useNetworkTaskManager == true)
            {
                taskManager = new JTaskCompletionSourceManager<IUnique>();
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

            if(networkMessageHandler == null)
            {
                throw new System.Exception("Message handler is required. Please set it using SetMessageHandler method.");
            }

            if (networkMessageProcessStrate == null)
            {
                networkMessageProcessStrate = new JNetworkMessageProcessStrate(netMessageSerializerStrate, messageTypeResolver, outProcesserManager, comingProcesserManager);
            }

            var network = new JNetwork(socketFactory,  networkMessageProcessStrate, networkMessageHandler);
            if(taskManager != null)
            {
                network.UseTaskManager(taskManager);
            }

            return network;
        }

        public JNetworkBuilder SetSocketFactory(ISocketFactory factory)
        {
            socketFactory = factory;
            return this;
        }

        public JNetworkBuilder SetTaskManager(IJTaskCompletionSourceManager<IUnique> manager)
        {
            taskManager = manager;
            return UseNetworkTaskManager(true);
        }

        public JNetworkBuilder SetMessageProcessStrate(INetworkMessageProcessStrate strate)
        {
            networkMessageProcessStrate = strate;
            return this;
        }

        public JNetworkBuilder SetMessageHandler(INetworkMessageHandler handler)
        {
            networkMessageHandler = handler;
            return this;
        }

        public JNetworkBuilder SetSocket(IJSocket socket)
        {
            this.socket = socket;
            return this;
        }

        public JNetworkBuilder SetNetMessageSerializerStrate(INetMessageSerializerStrate strate)
        {
            netMessageSerializerStrate = strate;
            return this;
        }

        public JNetworkBuilder SetMessageTypeResolver(IMessageTypeResolver resolver)
        {
            messageTypeResolver = resolver;
            return this;
        }

        public JNetworkBuilder SetOutDataProcesser(JDataProcesserManager processer)
        {
            outProcesserManager = processer;
            return this;
        }

        public JNetworkBuilder SetComingDataProcesser(JDataProcesserManager processer)
        {
            comingProcesserManager = processer;
            return this;
        }

        public JNetworkBuilder SetDataConverter(IDataConverter converter)
        {
            dataConverter = converter;
            return this;
        }

        public JNetworkBuilder SetProtocolRegister(ITypeRegister register)
        {
            protocolRegister = register;
            return this;
        }

        /// <summary>
        /// 是否使用网络任务管理器，默认不使用，如果使用，会在发送消息时，创建一个任务，并等待这个任务完成或者超时，任务完成的时机是在收到响应消息的时候，调用tcs.TrySetResult(message)来完成任务
        /// </summary>
        /// <param name="use"></param>
        /// <returns></returns>
        public JNetworkBuilder UseNetworkTaskManager(bool use = true)
        {
            useNetworkTaskManager = use;
            return this;
        }
    }
}
