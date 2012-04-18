# IP Cam Stream to OpenGL Demo

This code demonstrates how to open a mjpeg stream and display it on a fullscreen texture with OpenTK.

## Testing

If you have no ip camera available you can use your smartphone with an ip streaming app.
To not drain your battery while experimenting it's easy to caputre a short period of the mjpeg stream via vlc

    $ vlc -I dummy --run-time=10 http://192.168.0.142:8080/videofeed --sout=file/asf:test-stream.asf vlc://quit

and rebroadcast it on http://localhost:8080 with

    $ vlc  -I dummy  -vvv test-stream.asf -L --sout '#standard{access=http,mux=mpjpeg,dst=:8080}'