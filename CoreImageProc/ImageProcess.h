#pragma once
#include <opencv2\core.hpp>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>

#include <string>

using namespace std;
using namespace cv;

cv::Mat readOriginal(std::string fileName);

cv::Mat RotateAt(const cv::Mat& img, float grad);

cv::Mat resizeImg(const cv::Mat& img, float percent);

cv::Mat contrastAndBrightness(const cv::Mat& img, float contrast, int brightness);

