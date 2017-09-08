 //This is the main DLL file.

#include "stdafx.h"
#include <opencv2\imgproc.hpp>

#include "CoreWrapper.h"

#include "../CoreImageProc/ImageProcess.h"
//
#using <System.Drawing.dll>

#include <string>

using namespace std;
//
//#include <msclr\marshal_cppstd.h>
//
//using namespace msclr::interop;
//
//typedef unsigned char uchar;


void MarshalString(System::String ^ s, string& os)
{
	using namespace Runtime::InteropServices;
	const char* chars =
		(const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	os = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}

void MarshalString(System::String ^ s, wstring& os)
{
	using namespace Runtime::InteropServices;
	const wchar_t* chars =
		(const wchar_t*)(Marshal::StringToHGlobalUni(s)).ToPointer();
	os = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}

System::Drawing::Image^ CoreWrapper::ImageProc::readOriginalWrapper(System::String^ fileName)
{
	string str;

	MarshalString(fileName, str);

	//std::string str = msclr::interop::marshal_as<std::string>(fileName);
//
	auto srcImg = readOriginal(str);
	bool em = srcImg.empty();

	/*cv::Mat matImage;
	cvtColor(srcImg, matImage, CV_BGRA2RGBA);*/

	//GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
	//auto a =Drawing::Bitmap(matImage.cols, matImage.rows, 1, 32, matImage.data);
	
	//auto srcImg = cv::Mat();

	//int bytes = srcImg.cols* srcImg.rows;
	//byte^ rgbValues = gcnew byte[bytes];
	//byte r[] = new byte[bytes / 3];
	//byte g[] = new byte[bytes / 3];
	//byte b[] = new byte[bytes / 3];
/*
	Drawing::Bitmap^ bmpData = gcnew System::Drawing::Bitmap(srcImg.cols, srcImg.rows);

	for (int column = 0; column < bmpData->Height; column++)
	{
		for (int row = 0; row < bmpData->Width; row++)
		{
			auto p = bmpData->GetPixel(column, row);
				
		}
	}*/
	auto preview = convertToPreview(srcImg, 1000, 1);
	return this->convertMatToImage(preview);
	//return Drawing::Image::FromFile(fileName);
}



//void CoreWrapper::ImageProc::convertToPreview(cv::Mat & sourceImg, int sideLenght, Side side)
//{
//	cv::Size originalSize = sourceImg.size();
//	float ratio;
//	switch (side)
//	{
//		case CoreWrapper::Side::WIDTH:
//			ratio = originalSize.width / (float)sideLenght;
//			break;
//		case CoreWrapper::Side::HEIGHT:
//			ratio = originalSize.height/ (float)sideLenght;
//			break;
//		default:
//			ratio = 1;
//			break;
//	}
//
//	cv::Size newSize (originalSize.width*ratio, originalSize.height*ratio);
//	cv::Mat preview;
//	cv::resize(sourceImg, preview, newSize);
//	sourceImg = preview;
//}

Image ^ CoreWrapper::ImageProc::convertMatToImage(const cv::Mat & opencvImage)
{
	Drawing::Bitmap^ newImage = gcnew Drawing::Bitmap(opencvImage.cols, opencvImage.rows);
	for (size_t i = 0; i < opencvImage.rows - 1; i++)
	{
		const uchar* row = opencvImage.ptr(i);
		for (size_t j = 0; j < opencvImage.cols - 1; j++)
		{
			Drawing::Color c = Drawing::Color::FromArgb(255, row[j * 3 + 2], row[j * 3 + 1], row[j * 3]);
			newImage->SetPixel(j, i, c);
		}
	}
	return (Drawing::Image^)newImage;
}
