#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>

using namespace std;
using namespace cv;

class CoreImgEditor
{
public:
	CoreImgEditor(string fileName):
		percentRatio(1),
		contrast(1),
		brightness(0),
		rotateAngle(0)
	{
		source = cv::imread(fileName);
	}

	cv::Mat RotateAt(const cv::Mat img)
	{
			//Computing affine matrix
			auto center = Point2f(img.cols / 2, img.rows / 2);
			cv::Mat rotateMat = getRotationMatrix2D(center, rotateAngle, 1);
			auto rotRect = RotatedRect(center, source.size(), rotateAngle).boundingRect();
			rotateMat.at<double>(0, 2) += rotRect.width / 2.0 - center.x;
			rotateMat.at<double>(1, 2) += rotRect.height / 2.0 - center.y;
		
			cv::Mat rotatedImg;
			warpAffine(img, rotatedImg, rotateMat, rotRect.size());
			return rotatedImg;
	}

	cv::Mat resizeImg(const cv::Mat& img)
	{
			Size newSize(img.size().width*percentRatio, img.size().height*percentRatio);
			Mat resized;
			cv::resize(img, resized, newSize);
			return resized;
	}

	cv::Mat contrastAndBrightness(const cv::Mat& img)
	{
			Mat edited;
			img.convertTo(edited, edited.type(), contrast, brightness);
			return edited;
	}

	void changeContrastAndBrightness(float _contrast, int _brightness)
	{
		contrast = _contrast;
		brightness = _brightness;
		editSource();
	}

	void resize(float _percentRatio)
	{
		percentRatio = _percentRatio;
		editSource();
	}

	void rotate(float _rotateAngle)
	{
		rotateAngle = _rotateAngle;
		editSource();
	}

	cv::Mat getPreview()
	{
		return changed;
	}

private:

	void editSource()
	{
		changed = resizeImg(source);
		changed = RotateAt(changed);
		changed = contrastAndBrightness(changed);
	}

	cv::Mat source;
	cv::Mat changed;
	cv::Mat preview;

	float rotateAngle;
	float contrast;
	int brightness;
	float percentRatio;
};