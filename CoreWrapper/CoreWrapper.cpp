 //This is the main DLL file.

#include "stdafx.h"
#include <opencv2\imgproc.hpp>

#include "CoreWrapper.h"

#include "../CoreImageProc/ImageProcess.h"
//
#using <System.Drawing.dll>

#include <string>
#include <ctime>

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
	//editor->rotate(45);
	auto start = clock();
	editor->updatePreview(400, 400);
	auto end = clock() - start;
	auto srcImg = editor->getPreview();
	bool em = srcImg.empty();
	auto image = this->convertMatToImage(srcImg);
	return image;
}

void CoreWrapper::ImageProc::loadNewImage(System::String ^ fileName)
{
	string str;
	MarshalString(fileName, str);
	editor->loadImg(str);
}

void CoreWrapper::ImageProc::editImage(float _sizeRatio, float _rotateAngle, float _contrast, int _brightness)
{
	editor->editImage(_sizeRatio, _rotateAngle, _contrast, _brightness);
}

void CoreWrapper::ImageProc::editContrastAndBrightness(float _contrast, int _brightness)
{
	editor->changeContrastAndBrightness(_contrast, _brightness);
}

void CoreWrapper::ImageProc::rotateImage(float _grad)
{
	editor->rotate(_grad);
}

void CoreWrapper::ImageProc::resizeImage(float _ratio)
{
	editor->resize(_ratio);
}

CoreWrapper::ImageProc::ImageProc(System::String^ fileName)
{
	std::string str;
	MarshalString(fileName, str);
	editor = new CoreImgEditor(str, 400, 400);
}

Image ^ CoreWrapper::ImageProc::convertMatToImage(const cv::Mat & opencvImage)
{
	auto start = clock();
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
	auto end = clock() - start;
	return (Drawing::Image^)newImage;
}
