// CoreWrapper.h

#pragma once

#using <System.Drawing.dll>

using namespace System;
using namespace System::Drawing;

namespace CoreWrapper {

	enum Side{WIDTH, HEIGHT};

	public ref class ImageProc
	{
	public:

		int foo() { return 0; }

		System::Drawing::Image^ readOriginalWrapper(System::String^ fileName);

	private:
		//void convertToPreview(cv::Mat& sourceImg, int sideLenght, Side side);

		Image^ convertMatToImage(const cv::Mat& opencvImage);
		
	};
}
