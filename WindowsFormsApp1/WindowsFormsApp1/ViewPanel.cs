using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public class ViewPanel : Panel
	{
		private readonly Painter painter;
		private PointF centerLogicalPos;
		private float zoomScale;

		public ViewPanel(Painter painter)
			: this()
		{
			this.painter = painter;
		}

		public ViewPanel()
		{
			FitToWindow = true;
			zoomScale = 1f;
		}

		public PointF CenterLogicalPos
		{
			get { return centerLogicalPos; }
			set
			{
				centerLogicalPos = value;
				FitToWindow = false;
			}
		}

		public float ZoomScale
		{
			get { return zoomScale; }
			set
			{
				zoomScale = Math.Min(1000f, Math.Max(0.001f, value));
				FitToWindow = false;
			}
		}

		public bool FitToWindow { get; set; }

		protected override void InitLayout()
		{
			base.InitLayout();
			ResizeRedraw = true;
			DoubleBuffered = true;
		}

		private PointF GetShift()
		{
			return new PointF(
				ClientSize.Width / 2f - CenterLogicalPos.X * ZoomScale,
				ClientSize.Height / 2f - CenterLogicalPos.Y * ZoomScale);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.Clear(Color.Black);
			if (painter == null) return;
			var sceneSize = painter.Size;
			if (FitToWindow)
			{
				var vMargin = sceneSize.Height * ClientSize.Width < ClientSize.Height * sceneSize.Width;
				zoomScale = vMargin
					? ClientSize.Width / sceneSize.Width
					: ClientSize.Height / sceneSize.Height;
				centerLogicalPos = new PointF(sceneSize.Width / 2, sceneSize.Height / 2);
			}

			var shift = GetShift();
			e.Graphics.ResetTransform();
			e.Graphics.TranslateTransform(shift.X, shift.Y);
			e.Graphics.ScaleTransform(ZoomScale, ZoomScale);
			painter.Paint(e.Graphics);
		}
	}
}