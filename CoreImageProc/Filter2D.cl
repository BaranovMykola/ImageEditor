#if defined(cl_khr_fp64)
#  pragma OPENCL EXTENSION cl_khr_fp64: enable
#elif defined(cl_amd_fp64)
#  pragma OPENCL EXTENSION cl_amd_fp64: enable
#else
#  error double precision is not supported
#endif
kernel void AddWeighted(
	global const unsigned char *Src,
	global const float *Kern,
	global unsigned char *Dst,
	int rows,
	int cols,
	int anchX,
	int anchY,
	int kernRows,
	int kernCols,
	int channels
)
{
	size_t id = get_global_id(0);
	int offset = id%channels;
	int idC = id / 3;
	int y = (id) / (cols*channels);
	int x = ((id) % (cols*channels) )/channels;
	if (!(x < anchX || x >(cols - kernCols + anchX)
		||
		y < anchY || y > (rows - kernRows+anchY)))
	{
		double result = 0;
		for (int k = 0; k < kernRows; ++k)
		{
			for (int l = 0; l < kernCols; ++l)
			{
				int disoff = ((x + l - anchX)*channels) % channels;
				result += Kern[k*kernCols + l] * Src[(y + k - anchY)*cols*channels + (x+l- anchX)*channels+offset];
			}
		}
		int round;
		if (result - (int)result > 0.5)
		{
			round = result + 1;
		}
		else
		{
			round = result;
		}
		int sature = round < 0 ? 0 : round > 255 ? 255 : round;
		Dst[id] = sature;
	}
}