// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export enum RequestMethod {
    GET = 'GET',
    POST = 'POST',
    PUT = 'PUT',
    DELETE = 'DELETE'
}

export interface FetchOption<T> {
    url: string;
    method: RequestMethod;
    body?: BodyInit;
    headers?: HeadersInit;
    dispatch: (action: T) => void;
    onRequest?: Function;
    onComplete?: Function;
    onError?: Function;
}

export async function FetchService<T>(requestOption: FetchOption<T>) {
    try {
        if (requestOption.onRequest !== undefined) {
            requestOption.dispatch(requestOption.onRequest());
        }

        if (requestOption.onComplete !== undefined) {
            const response = await fetch(requestOption.url, {
                method: requestOption.method, body: requestOption.body, headers: requestOption.headers ?
                    requestOption.headers :
                    {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
            });
            if (response.status >= 400 && response.status < 600) {
                throw new Error("Bad response from server");
            }

            const data = await response.json();
            requestOption.dispatch(requestOption.onComplete(data));
        }

    } catch (errorMsg) {
        console.error(errorMsg);
        if ((errorMsg !== undefined) && requestOption.onError !== undefined) {
            requestOption.dispatch(requestOption.onError(errorMsg.message));
        }
    }
}