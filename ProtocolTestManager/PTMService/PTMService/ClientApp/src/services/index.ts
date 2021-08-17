// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export enum RequestMethod {
  GET = 'GET',
  POST = 'POST',
  PUT = 'PUT',
  DELETE = 'DELETE'
}

export interface FetchOption<T> {
  url: string
  method: RequestMethod
  body?: BodyInit
  headers?: HeadersInit
  dispatch: (action: T) => void
  onRequest?: Function
  onComplete?: Function
  onError?: Function
}

export async function FetchService<T> (requestOption: FetchOption<T>) {
  try {
    if (requestOption.onRequest !== undefined) {
      requestOption.dispatch(requestOption.onRequest())
    }

    if (requestOption.onComplete !== undefined) {
      const response = await fetch(requestOption.url, {
        method: requestOption.method,
        body: requestOption.body,
        headers: (requestOption.headers != null)
          ? requestOption.headers
          : {
              Accept: 'application/json',
              'Content-Type': 'application/json'
            }
      })
      if (response.status >= 400 && response.status < 600) {
        const data = await parseJson(response)
        if (data != '') {
          throw new Error(data)
        }
        throw new Error('Bad response from server')
      }

      const jsonHeader = response.headers.get('Content-Type')
      if (jsonHeader !== null && jsonHeader.includes('application/json')) {
        const data = await parseJson(response)
        requestOption.dispatch(requestOption.onComplete(data))

        return data
      }

      requestOption.dispatch(requestOption.onComplete())
      return await response.blob()
    }
  } catch (errorMsg) {
    console.error(errorMsg)
    if ((errorMsg !== undefined) && requestOption.onError !== undefined) {
      requestOption.dispatch(requestOption.onError(errorMsg.message))
    }
  }
}

async function parseJson (response: Response) {
  return await response.text().then(function (text: string) {
    return text ? JSON.parse(text) : {}
  })
}
