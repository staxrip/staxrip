/*****************************************************************************
* Copyright (c) 2012-2015 Fredrik Mellbin
* --- Legal stuff ---
* This program is free software. It comes without any warranty, to
* the extent permitted by applicable law. You can redistribute it
* and/or modify it under the terms of the Do What The Fuck You Want
* To Public License, Version 2, as published by Sam Hocevar. See
* http://sam.zoy.org/wtfpl/COPYING for more details.
*****************************************************************************/

#ifndef VSHELPER_H
#define VSHELPER_H

#include <limits.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>
#include <math.h>
#ifdef _WIN32
#include <malloc.h>
#endif
#include "VapourSynth.h"

/* Visual Studio doesn't recognize inline in c mode */
#if defined(_MSC_VER) && !defined(__cplusplus)
#define inline _inline
#endif

/* A kinda portable definition of the C99 restrict keyword (or its inofficial C++ equivalent) */
#if defined(__STDC_VERSION__) && __STDC_VERSION__ >= 199901L /* Available in C99 */
#define VS_RESTRICT restrict
#elif defined(__cplusplus) || defined(_MSC_VER) /* Almost all relevant C++ compilers support it so just assume it works */
#define VS_RESTRICT __restrict
#else /* Not supported */
#define VS_RESTRICT
#endif

#ifdef _WIN32
#define VS_ALIGNED_MALLOC(pptr, size, alignment) do { *(pptr) = _aligned_malloc((size), (alignment)); } while (0)
#define VS_ALIGNED_FREE(ptr) do { _aligned_free((ptr)); } while (0)
#else
#define VS_ALIGNED_MALLOC(pptr, size, alignment) do { if(posix_memalign((void**)(pptr), (alignment), (size))) *((void**)pptr) = NULL; } while (0)
#define VS_ALIGNED_FREE(ptr) do { free((ptr)); } while (0)
#endif

#define VSMAX(a,b) ((a) > (b) ? (a) : (b))
#define VSMIN(a,b) ((a) > (b) ? (b) : (a))

#ifdef __cplusplus 
/* A nicer templated malloc for all the C++ users out there */
#if __cplusplus >= 201103L || (defined(_MSC_VER) && _MSC_VER >= 1900)
template<typename T=void>
#else
template<typename T>
#endif
static inline T* vs_aligned_malloc(size_t size, size_t alignment) {
#ifdef _WIN32
    return (T*)_aligned_malloc(size, alignment);
#else
    void *tmp = NULL;
    if (posix_memalign(&tmp, alignment, size))
        tmp = 0;
    return (T*)tmp;
#endif
}

static inline void vs_aligned_free(void *ptr) {
    VS_ALIGNED_FREE(ptr);
}
#endif /* __cplusplus */

/* convenience function for checking if the format never changes between frames */
static inline int isConstantFormat(const VSVideoInfo *vi) {
    return vi->height > 0 && vi->width > 0 && vi->format;
}

/* convenience function to check for if two clips have the same format (unknown/changeable will be considered the same too) */
static inline int isSameFormat(const VSVideoInfo *v1, const VSVideoInfo *v2) {
    return v1->height == v2->height && v1->width == v2->width && v1->format == v2->format;
}

/* multiplies and divides a rational number, such as a frame duration, in place and reduces the result */
static inline void muldivRational(int64_t *num, int64_t *den, int64_t mul, int64_t div) {
    /* do nothing if the rational number is invalid */
    if (!*den)
        return;

    /* nobody wants to accidentally divide by zero */
    assert(div);

    int64_t a, b;
    *num *= mul;
    *den *= div;
    a = *num;
    b = *den;
    while (b != 0) {
        int64_t t = a;
        a = b;
        b = t % b;
    }
    if (a < 0)
        a = -a;
    *num /= a;
    *den /= a;
}

/* reduces a rational number */
static inline void vs_normalizeRational(int64_t *num, int64_t *den) {
    muldivRational(num, den, 1, 1);
}

/* add two rational numbers and reduces the result */
static inline void vs_addRational(int64_t *num, int64_t *den, int64_t addnum, int64_t addden) {
    /* do nothing if the rational number is invalid */
    if (!*den)
        return;

    /* nobody wants to accidentally add an invalid rational number */
    assert(addden);

    if (*den == addden) {
        *num += addnum;
    } else {
        int64_t temp = addden;
        addnum *= *den;
        addden *= *den;
        *num *= temp;
        *den *= temp;

        *num += addnum;

        vs_normalizeRational(num, den);
    }
}

/* converts an int64 to int with saturation, useful to silence warnings when reading int properties among other things */
static inline int int64ToIntS(int64_t i) {
    if (i > INT_MAX)
        return INT_MAX;
    else if (i < INT_MIN)
        return INT_MIN;
    else return (int)i;
}

static inline void vs_bitblt(void *dstp, int dst_stride, const void *srcp, int src_stride, size_t row_size, size_t height) {
    if (height) {
        if (src_stride == dst_stride && src_stride == (int)row_size) {
            memcpy(dstp, srcp, row_size * height);
        } else {
            const uint8_t *srcp8 = (const uint8_t *)srcp;
            uint8_t *dstp8 = (uint8_t *)dstp;
            size_t i;
            for (i = 0; i < height; i++) {
                memcpy(dstp8, srcp8, row_size);
                srcp8 += src_stride;
                dstp8 += dst_stride;
            }
        }
    }
}

/* check if the frame dimensions are valid for a given format */
/* returns non-zero for valid width and height */
static inline int areValidDimensions(const VSFormat *fi, int width, int height) {
    return !(width % (1 << fi->subSamplingW) || height % (1 << fi->subSamplingH));
}

/* Visual Studio doesn't recognize inline in c mode */
#if defined(_MSC_VER) && !defined(__cplusplus)
#undef inline
#endif

#endif
