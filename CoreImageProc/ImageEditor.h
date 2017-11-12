#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>
#include <vector>
#include <iostream>

#include "AbstractChange.h"
#include "ContrastAndBrightnessChange.h"
#include "RotateChange.h"
#include "ResizeChange.h"
#include "ImageProcessing.h"
#include "FaceDetactionChange.h"

using namespace cv;

class ImageEditor
{
public:
	ImageEditor()
	{
		currentChange = nullptr;
	}
	
	ImageEditor(std::string file)
	{
		currentChange = nullptr;
		loadImage(file);
	}

	void loadImage(std::string file)
	{
		original = imread(file);
		source = original.clone();
		preview = source.clone();
	}

	cv::Mat getSource()const
	{
		return source;
	}

	void changeContrastAndBrightness(float _contrast, int _brightness)
	{
		source.convertTo(preview, source.type(), _contrast, _brightness);
		eraseChange();
		currentChange = new ContrastAndBrightnessChange(_contrast, _brightness);
	}

	void restore(int step)
	{
		source = original.clone();
		for (int i = 0; i < step; i++)
		{
			changes[i]->apply(source);
		}
		changes.erase(changes.begin() + step + 1, changes.end());
		preview = source.clone();
	}

	void resize(float percentRatio)
	{
		preview = source.clone();
		imp::resize(preview, percentRatio);
		eraseChange();
		currentChange = new ResizeChange(percentRatio);
	}

	void rotate(int angle)
	{
		preview = source.clone();
		imp::rotate(preview, angle);
		eraseChange();
		currentChange = new RotateChange(angle);
	}

	void apply()
	{
		source = preview.clone();
		changes.push_back(currentChange);
		currentChange = nullptr;
	}

	Mat getPreview()const
	{
		return preview;
	}

	int getMinimum()const
	{
		double min;
		cv::minMaxLoc(source, &min, 0);
		return min;
	}

	std::vector<Mat> getPreviewIcons(float resizeRatio)
	{
		Mat originalCopy = original.clone();
		imp::resize(originalCopy, resizeRatio);

		std::vector<Mat> icons;
		icons.push_back(originalCopy.clone());
		for (int i = 0; i < changes.size(); i++)
		{
			changes[i]->apply(originalCopy);
			icons.push_back(originalCopy.clone());
		}

		for (auto i : icons)
		{
			imshow("change", i);
			waitKey();
		}
		return icons;
	}

	void save(std::string fileName)
	{
		if (original.empty())
		{
			imshow(fileName, Mat::zeros(300, 300, CV_8UC3));
		}
		imwrite(fileName, source);
	}

	void detectFace()
	{
		preview = source.clone();
		std::vector<Rect> faces;
		Mat frame_gray;

		Mat frame = preview;
		cvtColor(frame, frame_gray, CV_BGR2GRAY);
		equalizeHist(frame_gray, frame_gray);

		CascadeClassifier face_cascade;
		face_cascade.load("haarcascade_frontalface_alt2.xml");

		//-- Detect faces
		face_cascade.detectMultiScale(frame_gray, faces, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));

		for (size_t i = 0; i < faces.size(); i++)
		{
			Point center(faces[i].x + faces[i].width*0.5, faces[i].y + faces[i].height*0.5);
			rectangle(frame, faces[i], Scalar(0, 255, 0), 4);

			Mat faceROI = frame_gray(faces[i]);
			std::vector<Rect> eyes;
		}
		currentChange = new FaceDetectionChange(faces);
	}

public:

	void eraseChange()
	{
		if (currentChange != nullptr)
		{
			delete currentChange;
		}
	}
	Mat preview;
	Mat source;
	Mat original;
	std::vector<AbstractChange*> changes;
	AbstractChange* currentChange;
};
