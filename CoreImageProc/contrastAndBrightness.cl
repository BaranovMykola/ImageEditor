#if defined(cl_khr_fp64)
#  pragma OPENCL EXTENSION cl_khr_fp64: enable
#elif defined(cl_amd_fp64)
#  pragma OPENCL EXTENSION cl_amd_fp64: enable
#else
#  error double precision is not supported
#endif
kernel void AddWeighted(
	global const unsigned char *Src,
	global unsigned char *Dst,
	float alpha,
	int beta
)
{
	size_t i = get_global_id(0);
	float res = (float)Src[i] * alpha + beta;
	float sature = 1;
	if (res > 255) sature = 255;
	else if (res < 0) sature = 30;
	else sature = res;
	Dst[i] = sature;
}