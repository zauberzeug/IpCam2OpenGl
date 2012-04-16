using System;
using AForge.Video;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using TexLib;
using System.Drawing;

namespace IpCam2OpenGl
{
	class Window : GameWindow
	{
		Bitmap videoFrame;
		MJPEGStream videoStream;
		Quad fullscreenQuad = new Quad ();
        
		public Window ()
            : base(800, 800, GraphicsMode.Default, "Zauberzeug IpCam2OpenGL Demo")
		{
			VSync = VSyncMode.On;
		}

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			GL.Enable (EnableCap.DepthTest);
			TexUtil.InitTexturing ();

			GL.Hint (HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

			GL.DepthFunc (DepthFunction.Lequal);

			GL.ColorMaterial (MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
			GL.Enable (EnableCap.ColorMaterial);
			GL.Enable (EnableCap.PolygonSmooth);

			GL.Enable (EnableCap.Blend);
			GL.BlendFunc (BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.Ext.BindFramebuffer (FramebufferTarget.FramebufferExt, 0); // render per default onto screen, not some FBO
		
			OpenVideoStream ();
		}
		
		private void OpenVideoStream ()
		{
			string url = "http://192.168.0.142:8080/videofeed";
			
			Console.WriteLine ("Connecting to {0}", url);
			videoStream = new MJPEGStream (url);
			
			videoStream.NewFrame += (Object sender, NewFrameEventArgs eventArgs) => {
				Console.WriteLine ("updating frame");
				lock (videoFrame) {					
					videoFrame = eventArgs.Frame;
				}};
			
			videoStream.Start ();
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
			
			if (Keyboard [Key.Escape])
				Exit ();
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.BindFramebuffer (FramebufferTarget.Framebuffer, 0); // use the visible framebuffer
			GL.ClearColor (0.5f, 0.5f, 0.55f, 0.0f);
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Viewport (ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			Matrix4 modelview = Matrix4.LookAt (Vector3.Zero, -1 * Vector3.UnitZ, Vector3.UnitY);
			GL.MatrixMode (MatrixMode.Modelview);
			GL.LoadMatrix (ref modelview);

			GL.MatrixMode (MatrixMode.Projection);
			GL.LoadIdentity ();
			GL.Ortho (-1, 1, -1, 1, -1, 1.1);
			
			int videoTexture;
			if (videoFrame != null) {
				Console.WriteLine ("drawing frame");
				lock (videoFrame) {
					videoTexture = TexUtil.CreateTextureFromBitmap (videoFrame);
				}
				
				GL.BindTexture (TextureTarget.Texture2D, videoTexture);
			}
				fullscreenQuad.Draw ();
			
			SwapBuffers ();
		}
		
		protected override void OnClosing (System.ComponentModel.CancelEventArgs e)
		{
			
			videoStream.SignalToStop ();
			Console.WriteLine ("Disconnecting from {0}", videoStream.Source);
			videoStream.WaitForStop ();
			
			base.OnClosing (e);
		}

		[STAThread]
		static void Main ()
		{
			using (Window view = new Window()) {
				view.Run (30.0);
			}
		}
		
	}
}
