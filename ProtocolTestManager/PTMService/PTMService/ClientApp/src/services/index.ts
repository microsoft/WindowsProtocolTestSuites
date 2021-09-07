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
  onCompleteCallback?: Function
}

export async function FetchService<T>(requestOption: FetchOption<T>) {
  let isCallSucceded: boolean = true
  let result: any = undefined
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
        if (data !== '') {
          throw new Error(data)
        }
        throw new Error('Bad response from server')
      }

      const jsonHeader = response.headers.get('Content-Type')
      if (jsonHeader?.includes('application/json') ?? false) {
        const data = await parseJson(response)
        requestOption.dispatch(requestOption.onComplete(data))
        result = data
        return data
      }

      // TODO: Find out how to pass in a useful onComplete callback when the response isn't json
      requestOption.dispatch(requestOption.onComplete())
      isCallSucceded = true
      return await response.blob()
    }
  } catch (error) {
    isCallSucceded = false
    console.error(error)
    if ((error !== undefined) && requestOption.onError !== undefined) {
      requestOption.dispatch(requestOption.onError(error.message))
    }
  } finally {
    if (isCallSucceded && requestOption.onCompleteCallback !== undefined) {
      requestOption.onCompleteCallback(result)
    }
  }
}

async function parseJson(response: Response) {
  return await response.text().then(function (text: string) {
    return text ? JSON.parse(text) : {}
  })
}
