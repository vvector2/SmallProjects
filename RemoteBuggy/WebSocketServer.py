from websocket_server import WebsocketServer

class RobotWebSocketServer:
    def __init__(self,host, websocketPort, robot):
        self.__host = host
        self.__port = port
        self.__robot = robot

    def __new_client(self,client, server):
        print("New client (%s) connected" % client['id'])

    def __msg_received(self,client, server, msg):
        print("Client (%s) : %s" % (client['id'], msg))
        self.__robotControl(msg)

    def __client_left(self,client, server):
        print("Client (%s) left" % client['id'])
        self.__robot.TurnOff()

    def __robotControl(self,msg):
        if msg == 'w':
            self.__robot.Forward()
            self.__robot.TurnOn()
        elif msg == 's':
            self.__robot.Backward()
            self.__robot.TurnOn()
        elif msg == 'a':
            self.__robot.Left()
            self.__robot.TurnOn()
        elif msg == 'd':
            self.__robot.Right()
            self.__robot.TurnOn()
        elif msg == 'q':
            self.__robot.TurnOff()


    def startWebsocketServer(self):
        server = WebsocketServer(self.port, host = self.__host)
        server.set_fn_client_left(self.__client_left)
        server.set_fn_new_client(self.__new_client)
        server.set_fn_message_received(self.__msg_received)
        server.run_forever()
