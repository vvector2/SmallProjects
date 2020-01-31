class RobotBuggy:
    def __init__(self, GPIO):
        self.__GPIO = GPIO
        
        self.__GPIO.setmode(GPIO.BCM)
        self.__GPIO.setup(13, self.__GPIO.OUT)
        self.__GPIO.setup(21, self.__GPIO.OUT)

        self.__GPIO.setup(19, self.__GPIO.OUT)
        self.__GPIO.setup(26, self.__GPIO.OUT)

        self.__GPIO.setup(20, self.__GPIO.OUT)
        self.__GPIO.setup(16, self.__GPIO.OUT)

        self.__leftMotor = self.__GPIO.PWM(13, 1)
        self.__rightMotor = self.__GPIO.PWM(21, 1)

    def Forward(self):
        print('RobotBuggy: forward')
        self.__GPIO.output(19, self.__GPIO.HIGH)
        self.__GPIO.output(26, self.__GPIO.LOW)
        self.__GPIO.output(16, self.__GPIO.HIGH)
        self.__GPIO.output(20, self.__GPIO.LOW)

    def Backward(self):
        print('RobotBuggy: backward')
        self.__GPIO.output(19, self.__GPIO.LOW)
        self.__GPIO.output(26, self.__GPIO.HIGH)
        self.__GPIO.output(16, self.__GPIO.LOW)
        self.__GPIO.output(20, self.__GPIO.HIGH)

    def Right(self):
        print('RobotBuggy: right')
        self.__GPIO.output(19, self.__GPIO.HIGH)
        self.__GPIO.output(26, self.__GPIO.LOW)
        self.__GPIO.output(16, self.__GPIO.LOW)
        self.__GPIO.output(20, self.__GPIO.HIGH)

    def Left(self):
        print('RobotBuggy: left')
        self.__GPIO.output(19, self.__GPIO.LOW)
        self.__GPIO.output(26, self.__GPIO.HIGH) 
        self.__GPIO.output(16, self.__GPIO.HIGH)
        self.__GPIO.output(20, self.__GPIO.LOW)

    def TurnOn(self):
        print('RobotBuggy: turn on engine')
        self.__leftMotor.start(100)
        self.__rightMotor.start(87)

    def TurnOff(self):
        print('RobotBuggy: turn off engine')
        self.__leftMotor.stop()
        self.__rightMotor.stop()

