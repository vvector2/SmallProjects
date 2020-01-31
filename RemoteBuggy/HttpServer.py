from http.server import HTTPServer, SimpleHTTPRequestHandler

def CreateGetHandlerClass(cameraPiStreamingOutput):
   
    class GetHandler(SimpleHTTPRequestHandler):
        def __init__(self, *args, directory=None,**kwargs):
            super().__init__(*args, directory=directory, **kwargs)
            self.__cameraPiStreamingOutput = cameraPiStreamingOutput

        def do_GET(self):
            if self.path == '/stream.mjpg':
                self.send_response(200)
                self.send_header('Age', 0)
                self.send_header('Cache-Control', 'no-cache, private')
                self.send_header('Pragma', 'no-cache')
                self.send_header('Content-Type', 'multipart/x-mixed-replace; boundary=FRAME')
                self.end_headers()
                try:
                    output = self.__cameraPiStreamingOutput
                    while True:
                        with output.condition:
                            output.condition.wait()
                            frame = output.frame
                        self.wfile.write(b'--FRAME\r\n')
                        self.send_header('Content-Type', 'image/jpeg')
                        self.send_header('Content-Length', len(frame))
                        self.end_headers()
                        self.wfile.write(frame)
                        self.wfile.write(b'\r\n')
                except Exception as e:
                    print('Removed streaming client %s: %s', self.client_address, str(e))
            else:
                super(GetHandler,self).do_GET()
    
    return GetHandler


def StartHttpServer(host, port , cameraPiStreamingOutput):
    with HTTPServer((host,port), CreateGetHandlerClass(cameraPiStreamingOutput)) as httpd:
        print("serving at port", port)
        httpd.serve_forever()
