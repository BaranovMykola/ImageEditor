#include <opencv2\core.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>

#include "ImageProcess.h"
#include "ImageEditor.h"

int main()
{
	std::string act;
	ImageEditor edit("fox.jpg");
	do
	{
		cin >> act;
		if (act == "show")
		{
			imshow("", edit.source);
			waitKey();
			destroyAllWindows();
		}
		else if (act == "change")
		{
			float c;
			int b;
			cout << "c >> b >>" << endl;
			cin >> c >> b;
			edit.changeContrastAndBrightness(c, b);
		}
		else if (act == "restore")
		{
			int s;
			cout << "s >>" << endl;
			cin >> s;
			edit.restore(s);
		}
	}
	while (true);
	return 0;
}