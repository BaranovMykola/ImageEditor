#pragma once
#include <opencv2\core.hpp>
#include <opencv2\imgproc.hpp>
#include <opencv2\highgui.hpp>

#include "opencv2/objdetect/objdetect.hpp"

using namespace cv;
namespace imp
{
	void resize(cv::Mat& source, float percentRatio)
	{
		cv::Size newSize(source.size().width*percentRatio, source.size().height*percentRatio);
		cv::Mat resized = Mat::zeros(newSize, source.type());
		cv::resize(source, source, newSize);
	}

	void rotate(cv::Mat& source, int angle)
	{
		auto center = Point2f(source.cols / 2, source.rows / 2);
		cv::Mat rotateMat = getRotationMatrix2D(center, angle, 1);
		auto rotRect = RotatedRect(center, source.size(), angle).boundingRect();
		rotateMat.at<double>(0, 2) += rotRect.width / 2.0 - center.x;
		rotateMat.at<double>(1, 2) += rotRect.height / 2.0 - center.y;

		cv::Mat rotatedImg = Mat::zeros(source.size(), source.type());
		warpAffine(source, rotatedImg, rotateMat, rotRect.size()-Size(1,1), 1, BORDER_TRANSPARENT);
		source = rotatedImg;
	}

	void palleting256(cv::Mat& source)
	{
		for (int i = 0; i < source.rows; i++)
		{
			for (int j = 0; j < source.cols*source.channels(); j++)
			{
				int val = source.at<uchar>(i, j);
				int diff = val%43;
				val -= diff;
				source.at<uchar>(i, j) = val;
			}
		}
	}

	void filter2D(cv::Mat& source, cv::Mat& kern, cv::Point anchor)
	{
		Mat filtered;
		cv::filter2D(source, filtered, source.depth(), kern, anchor);
		source = filtered;
	}

	void toGrayscale(Mat& source)
	{
		if (source.type() == CV_8UC3)
		{
			cv::cvtColor(source, source, CV_BGR2GRAY);
		}
	}

	void printFaces(cv::Mat& source, std::vector<cv::Rect> faces)
	{
		for (auto i : faces)
		{
			cv::rectangle(source, i, cv::Scalar(0, 255, 0), 4);
		}
	}

	std::vector<cv::Rect> detectFace(Mat& source)
	{
		std::vector<Rect> faces;
		Mat frame_gray;

		Mat frame = source;
		if (frame.type() == CV_8UC1)
		{
			frame_gray = frame.clone();
		}
		else
		{
			cvtColor(frame, frame_gray, CV_BGR2GRAY);
		}

		equalizeHist(frame_gray, frame_gray);

		CascadeClassifier face_cascade;
		bool loaded = face_cascade.load("haarcascade_frontalface_alt2.xml");
		cout << "Loaded " << loaded << endl;


		//-- Detect faces
		face_cascade.detectMultiScale(frame_gray, faces, 1.1, 2, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));

		printFaces(source, faces);

		return faces;
	}
}