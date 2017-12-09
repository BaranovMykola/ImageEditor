 //This is the main DLL file.

#include "stdafx.h"
#include <opencv2\imgproc.hpp>

#include "CoreWrapper.h"
//#include <windows.h>
//#include <windef.h>

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

Bitmap ^ CoreWrapper::ImageProc::getPreview()
{
	auto img = editor->getPreview();
	//cv::imshow("img", img);
	//waitKey();
	return ConvertMatToBitmap(img);
}

System::Collections::Generic::List<Bitmap^>^ CoreWrapper::ImageProc::getPreivewIcons(float resizeRatio)
{
	auto icons = editor->getPreviewIcons(resizeRatio);
	System::Collections::Generic::List<Bitmap^>^ iconsList = gcnew System::Collections::Generic::List<Bitmap^>();
	for (Mat i : icons)
	{
		iconsList->Add(ConvertMatToBitmap(i));
	}
	return iconsList;
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

void CoreWrapper::ImageProc::applyRotate(float angle)
{
	editor->rotate(angle);
}

void CoreWrapper::ImageProc::applyResize(float ratio)
{
	editor->resize(ratio);
}

int CoreWrapper::ImageProc::getMinimumOfImage()
{
	return editor->getMinimum();
}

void CoreWrapper::ImageProc::apply()
{
	editor->apply();
}

void CoreWrapper::ImageProc::restore(int changeIndex)
{
	editor->restore(changeIndex);
}

void CoreWrapper::ImageProc::save(System::String ^ fileName)
{
	std::string stdFileName;
	MarshalString(fileName, stdFileName);
	editor->save(stdFileName);
}

void CoreWrapper::ImageProc::detectFace()
{
	editor->detectFace();
	editor->apply();
}

void CoreWrapper::ImageProc::paletting()
{
	editor->palleting256();
	editor->apply();
}

void CoreWrapper::ImageProc::filter(FilterEntity::Filter^ filterInstance)
{
	int rows = filterInstance->Matrix->Count;
	int cols = filterInstance->Matrix[0]->Count;
	cv::Mat kern = cv::Mat::zeros(cv::Size(cols, rows), CV_32F);
	cv::Point anchor;
	for (int i = 0; i < rows; i++)
	{
		for (int j = 0; j < cols; j++)
		{
			auto mat = (filterInstance->Matrix);
			auto row = mat[i];
			auto item = row[j];
			if (item->IsAnchor)
			{
				anchor = cv::Point(j, i);
			}
			kern.at<float>(cv::Point(j, i)) = item->Coeficient;
		}
	}
	editor->filter(kern, anchor);
	editor->apply();
}

void CoreWrapper::ImageProc::toGrayScale()
{
	editor->toGrayscale();
	editor->apply();
}

Bitmap^ CoreWrapper::ImageProc::ConvertMatToBitmap(cv::Mat img)
{
	if (img.type() != CV_8UC3)
	{
		Mat imgColor;
		cvtColor(img, imgColor, CV_GRAY2BGR);
		img = imgColor;
	}

	System::Drawing::Imaging::PixelFormat fmt(System::Drawing::Imaging::PixelFormat::Format24bppRgb);
	Bitmap ^bmpimg = gcnew Bitmap(img.cols, img.rows, fmt);
	System::Drawing::Imaging::BitmapData ^data = bmpimg->LockBits(System::Drawing::Rectangle(0, 0, img.cols, img.rows), System::Drawing::Imaging::ImageLockMode::WriteOnly, fmt);
	char *dstData = reinterpret_cast<char*>(data->Scan0.ToPointer());
	unsigned char *srcData = img.data;
	for (int row = 0; row < data->Height; ++row)
	{
		memcpy(reinterpret_cast<void*>(&dstData[row*data->Stride]), reinterpret_cast<void*>(&srcData[row*img.step]), img.cols*img.channels());
	}

	bmpimg->UnlockBits(data);
	return bmpimg;
}