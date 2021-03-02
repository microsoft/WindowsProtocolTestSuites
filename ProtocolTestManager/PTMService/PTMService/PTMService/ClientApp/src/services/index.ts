// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export enum RequestMethod {
    GET = 'GET',
    POST = 'POST',
    PUT = 'PUT',
    DELETE = 'DELETE'
}
export async function FetchService(url: string, requestMethod: RequestMethod, request?: any, complete?: any, error?: any) {
    try {
        if (request !== undefined) {
            request();
        }

        if (complete !== undefined) {
            const response = await fetch(url, { method: requestMethod });
            if (response.status >= 400 && response.status < 600) {
                throw new Error("Bad response from server");
            }

            const data = await response.json();
            complete(data);
        }

    } catch (errorMsg) {
        if (errorMsg !== undefined) {
            error(errorMsg)
        }
    }
}