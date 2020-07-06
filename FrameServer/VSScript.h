/*
* Copyright (c) 2013-2018 Fredrik Mellbin
*
* This file is part of VapourSynth.
*
* VapourSynth is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
* VapourSynth is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with VapourSynth; if not, write to the Free Software
* Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
*/

#ifndef VSSCRIPT_H
#define VSSCRIPT_H

#include "VapourSynth.h"

#define VSSCRIPT_API_MAJOR 3
#define VSSCRIPT_API_MINOR 2
#define VSSCRIPT_API_VERSION ((VSSCRIPT_API_MAJOR << 16) | (VSSCRIPT_API_MINOR))

/* As of api 3.2 all functions are threadsafe */

typedef struct VSScript VSScript;

typedef enum VSEvalFlags {
    efSetWorkingDir = 1,
} VSEvalFlags;

/* Get the api version */
VS_API(int) vsscript_getApiVersion(void); /* api 3.1 */

/* Initialize the available scripting runtimes, returns zero on failure */
VS_API(int) vsscript_init(void);

/* Free all scripting runtimes */
VS_API(int) vsscript_finalize(void);

/*
* Pass a pointer to a null handle to create a new one
* The values returned by the query functions are only valid during the lifetime of the VSScript
* scriptFilename is if the error message should reference a certain file, NULL allowed in vsscript_evaluateScript()
* core is to pass in an already created instance so that mixed environments can be used,
* NULL creates a new core that can be fetched with vsscript_getCore() later OR implicitly uses the one associated with an already existing handle when passed
* If efSetWorkingDir is passed to flags the current working directory will be changed to the path of the script
* note that if scriptFilename is NULL in vsscript_evaluateScript() then __file__ won't be set and the working directory won't be changed
* Set efSetWorkingDir to get the default and recommended behavior
*/
VS_API(int) vsscript_evaluateScript(VSScript **handle, const char *script, const char *scriptFilename, int flags);
/* Convenience version of the above function that loads the script from a file */
VS_API(int) vsscript_evaluateFile(VSScript **handle, const char *scriptFilename, int flags);
/* Create an empty environment for use in later invocations, mostly useful to set script variables before execution */
VS_API(int) vsscript_createScript(VSScript **handle);

VS_API(void) vsscript_freeScript(VSScript *handle);
VS_API(const char *) vsscript_getError(VSScript *handle);
/* The node returned must be freed using freeNode() before calling vsscript_freeScript() */
VS_API(VSNodeRef *) vsscript_getOutput(VSScript *handle, int index);
/* Both nodes returned must be freed using freeNode() before calling vsscript_freeScript(), the alpha node pointer will only be set if an alpha clip has been set in the script */
VS_API(VSNodeRef *) vsscript_getOutput2(VSScript *handle, int index, VSNodeRef **alpha); /* api 3.1 */
/* Unset an output index */
VS_API(int) vsscript_clearOutput(VSScript *handle, int index);
/* The core is valid as long as the environment exists */
VS_API(VSCore *) vsscript_getCore(VSScript *handle);
/* Convenience function for retrieving a vsapi pointer */
VS_API(const VSAPI *) vsscript_getVSApi(void); /* deprecated as of api 3.2 since it's impossible to tell the api version supported */
VS_API(const VSAPI *) vsscript_getVSApi2(int version); /* api 3.2, generally you should pass VAPOURSYNTH_API_VERSION */

/* Variables names that are not set or not of a convertible type will return an error */
VS_API(int) vsscript_getVariable(VSScript *handle, const char *name, VSMap *dst);
VS_API(int) vsscript_setVariable(VSScript *handle, const VSMap *vars);
VS_API(int) vsscript_clearVariable(VSScript *handle, const char *name);
/* Tries to clear everything set in an environment, normally it is better to simply free an environment completely and create a new one */
VS_API(void) vsscript_clearEnvironment(VSScript *handle);

#endif /* VSSCRIPT_H */
