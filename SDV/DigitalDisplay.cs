// Digital Clock Display by Sriram Chitturi (c) Copyright 2004
// A class to display digital digits and characters on a rectangular area

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SDV
{
	// A digit class which can draw a number on a graphics surface
	internal sealed class DigitalDisplay
	{
		// pens used to draw the digits
		private static Pen pen = new Pen(Color.FromArgb(255,0,0));
		private static Pen dimPen = null;

		private float linewidth = 20.0F;
		private Point[] Points; // end point coordinates of lines in the digital display

		// for each digit the display bits are set into an int
		// 'A' and 'P' are included for AM and PM display in the clock
		private int[] displayNum = new int[12]{63,12,118,94,77,91,123,14,127,95,
												  111, // to display 'A'
												  103}; // to display 'P'

		// Rectangles in which colons are displayed
		private Rectangle colonRect1, colonRect2;
	
		// This function is called by the paint method to display the numbers
		// A set of bits in the 'displayNum' variable define which of the
		// display legs to display
		// Based on this the ones with a '1' are in bright color and the rest
		// with '0's are in a dull color giving the effect of a digital clock
		internal void Draw(int num,  // number to display
			Graphics g) // graphics object for drawing
		{
			int check; // used to check if a leg of digit should be bright or dull

			// although pens are global linewidths are specific to each instance
			pen.Width = dimPen.Width = linewidth;

			for (int i=0; i<7; i++)
			{
				check = (int)System.Math.Pow(2, i);
				if ((check & displayNum[num])==0)
					g.DrawLine(dimPen, Points[i*2], Points[i*2+1]);
				else
					g.DrawLine(pen, Points[i*2], Points[i*2+1]);
			}
		}

		static private void setDimPen()
		{
			if (dimPen == null) dimPen = (Pen)pen.Clone();
			dimPen.Width = pen.Width;
        
            dimPen.Color = Color.FromArgb(
                pen.Color.R > 0 ? 60 : 0,
                pen.Color.G > 0 ? 60 : 0,
                pen.Color.B > 0 ? 60 : 0);
		}

		static internal void SetPenColor(DigitalColor dclr)
		{
            if (dclr == DigitalColor.Black)
            {
                pen.Color = Color.Black;
            }
            else if (dclr == DigitalColor.White)
            {
                pen.Color = Color.White;
            }
            else
            {

                pen.Color = Color.FromArgb(
                    (dclr == DigitalColor.RedColor) ? 255 : 0,
                    (dclr == DigitalColor.GreenColor) ? 255 : 0,
                    (dclr == DigitalColor.BlueColor) ? 255 : 0);
            }
			setDimPen();
		}

		// function that draws a colon in the middle of the rectangular panel
		// possible modes are circular or rectangular points in the colon
		internal void DrawColon(Graphics g, ColonType type, bool dim)
		{
			pen.Width = dimPen.Width = linewidth;
			Pen p = (dim) ? dimPen : pen; // choose a pen for blinking
			switch(type)
			{
				case ColonType.Circular:
					g.DrawEllipse(p, colonRect1);
					g.DrawEllipse(p, colonRect2);
					break;
				case ColonType.Rectangular:
					g.DrawRectangle(p, colonRect1);
					g.DrawRectangle(p, colonRect2);
					break;
			}
		}

		// Draws the complete rectangle in dim shade to give the digital effect :-)
		internal void Draw(Graphics g)
		{
			// althought pens are static, linewidths are specific to each instance
			dimPen.Width = linewidth;
			for (int i=0; i<7; i++)
				g.DrawLine(dimPen, Points[i*2], Points[i*2+1]);
		}

		// Overloaded function to display characters 'A' and 'P' for AM and PM
		// Using the same algorithm used to display numbers above
		internal void Draw(char ch, // character to display
						Graphics g) // graphics object for drawing
		{
			// 10 and 11 are indices of A and P in the displayNum array
			switch(Char.ToUpper(ch))
			{
				case 'A':
					Draw(10, g);
					break;
				case 'P':
					Draw(11, g);
					break;
			}
		}

		// Constructor takes a rectangle and prepares the end points
		// of the lines to be drawn for the clock
		internal DigitalDisplay(Rectangle rect)
		{
			pen.StartCap = LineCap.Triangle;
			pen.EndCap = LineCap.Triangle;

			Points = new Point[14]; // there are 7 lines in a display
			for (int i=0; i<14; i++)
				Points[i] = new Point(0,0);
			CalculateAllParameters(rect);
		}

		internal void CalculateAllParameters(Rectangle rect)
		{
			linewidth = (int)(rect.Width/5);
			if (linewidth < 2) linewidth = 2;
			if (linewidth > 20) linewidth = 20;
			pen.Width = linewidth;
			setDimPen();

			CalculateLineEnds(rect);
			CalculateColonRectangles(rect);
		}

		// Function calculates end points of lines to display
		// The draw function will draw lines using this data
		private void CalculateLineEnds(Rectangle rect)
		{
			// 0,1,2,9,10,11,12 points share the same left edge X coordinate
			Points[0].X = Points[1].X = Points[2].X = Points[9].X = 
				Points[10].X = Points[11].X = Points[12].X = rect.Left;
 
			// points 3,4,5,6,7,8,13 the right edge X coordinate
			Points[3].X = Points[4].X = Points[5].X = Points[6].X =
					Points[7].X = Points[8].X = Points[13].X= rect.Right-(int)linewidth;

			// Points 1,2,3,4 are the top most points
			Points[1].Y = Points[2].Y = Points[3].Y = Points[4].Y = (int)(rect.Top);

			// Points 0,11,12,13,5,6 are the middle points
			Points[0].Y = Points[11].Y = Points[12].Y = Points[13].Y =
						Points[5].Y = Points[6].Y = 
							rect.Top + (int)((rect.Height-linewidth)/2.0);
			// points 7,8,9,10 are on the bottom edge
			Points[7].Y = Points[8].Y = Points[9].Y = Points[10].Y 
							= rect.Top + (int)(rect.Height-linewidth);

			// now adjust the coordinates that were computed, to get the digital look
			AdjustCoordinates();
		}
	
		// This function is necessary to give the lines a digital clock look
		// Push the coordinates a little away so that they look apart
		private void AdjustCoordinates()
		{
			Point swap; // required in case points have to be swapped
			for (int i=0; i<7; i++)
			{
				// Always draw from left to right and top to bottom
				// Adjust the end points accordingly
				if (Points[i*2].X > Points[(i*2)+1].X || Points[i*2].Y > Points[(i*2)+1].Y)
				{
					swap = Points[i*2]; Points[i*2]= Points[(i*2)+1]; Points[(i*2)+1]=swap;
				}

				// for horizontal lines adjust the X coord
				if (Points[i*2].X != Points[(i*2)+1].X)
				{
					Points[i*2].X += (int)(linewidth/1.6);
					Points[(i*2)+1].X -= (int)(linewidth/1.6);
				}
				// for vertical lines adjust the y coord
				if (Points[i*2].Y != Points[(i*2)+1].Y)
				{
					Points[i*2].Y += (int)(linewidth/1.6);
					Points[(i*2)+1].Y -= (int)(linewidth/1.6);
				}
			}
		}

		// function to calculate the rectangles required to drawn colon dot inside
		private void CalculateColonRectangles(Rectangle rect)
		{
			colonRect1 = colonRect2 = rect;
			colonRect1.X = colonRect2.X = rect.X + (int)((rect.Width - linewidth)/2.0);
			colonRect1.Y = rect.Y + rect.Height/3;
			colonRect2.Y = rect.Y + (rect.Height*2)/3;
			colonRect1.Width = colonRect1.Height = 
				colonRect2.Width = colonRect2.Height = (int) linewidth;
		}
	}
}
