using System;
using AForge.Video;

namespace IpCam2OpenGl
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string url = "http://192.168.178.127:8080/videofeed";
			
			Console.WriteLine ("Connecting to {0}", url);
			MJPEGStream source = new MJPEGStream (url);
			
			source.NewFrame += (Object sender, NewFrameEventArgs eventArgs) => {
				Console.WriteLine ("new frame: {0}", eventArgs);
			};
			
			source.Start();
			source.WaitForStop();
		}
	}
}
