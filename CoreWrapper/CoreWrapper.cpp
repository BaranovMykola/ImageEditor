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
	std::string str;
	MarshalString(fileName, str);
	editor->loadImg(str);
	editor->rotate(45);
	auto srcImg = editor->getPreview();
	bool em = srcImg.empty();
	auto image = this->convertMatToImage(srcImg);
	return image;
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

CoreWrapper::ImageProc::ImageProc(System::String^ fileName)
{
	editor = new CoreImgEditor();
}

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
