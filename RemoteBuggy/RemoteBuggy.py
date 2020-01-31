import threading
import RPi.GPIO as GPIO
import picamera
from RobotBuggy import RobotBuggy
from WebSocketServer import StartWebsocketServer
from HttpServer import StartHttpServer
from StreamingOutput import StreamingOutput

httpPort = 65431
websocketPort = 65432
host ='192.168.0.17'

try:
    robot = RobotBuggy(GPIO)

    camera = picamera.PiCamera(resolution='640x480', framerate=24)
    streamingOutput = StreamingOutput()
    camera.start_recording(streamingOutput, format='mjpeg')

    httpServerThread =threading.Thread(target=StartHttpServer,args=[host,httpPort,streamingOutput],daemon=True)
    httpServerThread.start()

    StartWebsocketServer(host,websocketPort)
except KeyboardInterrupt:  
    print('Servers stop')

except Exception as e:
    print('Server encounters a problem. ', e) 

finally:
    camera.close()
    robot.TurnOff()
    GPIO.cleanup()
