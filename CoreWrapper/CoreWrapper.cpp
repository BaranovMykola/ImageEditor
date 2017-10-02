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

Bitmap^ CoreWrapper::ImageProc::convertMatToImage(const cv::Mat & opencvImage)
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
	return newImage;
}


CoreWrapper::ImageProc::ImageProc()
{
	editor = new ImageEditor();
}

Bitmap ^ CoreWrapper::ImageProc::getSource()
{
	auto img = editor->getSource();
	return ConvertMatToBitmap(img);
}

void CoreWrapper::ImageProc::loadImage(System::String ^ file)
{
	std::string stdFileName;
	MarshalString(file, stdFileName);
	editor->loadImage(stdFileName);
}

void CoreWrapper::ImageProc::applyContrastAndBrightness(float contrast, int brightness)
{
	editor->changeContrastAndBrightness(contrast, brightness);
}

Bitmap^ CoreWrapper::ImageProc::ConvertMatToBitmap(cv::Mat matToConvert)
{
	return gcnew Bitmap(matToConvert.cols, matToConvert.rows, matToConvert.step, System::Drawing::Imaging::PixelFormat::Format24bppRgb, IntPtr(matToConvert.ptr()));
}