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
  let isCallSuccessful: boolean = true
  let result: T | Blob | undefined = undefined
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
          : { Accept: 'application/json', 'Content-Type': 'application/json' }
      })
      if (response.status >= 400 && response.status < 600) {
        const data = await parseResponseBody(response)
        throw new Error(getErrorMessage(data, response.status))
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
      result = await response.blob()
      return result
    }
  } catch (error: any) {
    isCallSuccessful = false
    console.error(error)
    if ((error !== undefined) && requestOption.onError !== undefined) {
      requestOption.dispatch(requestOption.onError(error.message))
    }
  } finally {
    if (isCallSuccessful && requestOption.onCompleteCallback !== undefined) {
      await requestOption.onCompleteCallback(result)
    }
  }
}

async function parseJson(response: Response) {
  return await response.text().then(function (text: string) {
    return text ? JSON.parse(text) : {}
  })
}

async function parseResponseBody(response: Response): Promise<unknown> {
  const text = await response.text()
  if (!text) {
    return undefined
  }

  try {
    return JSON.parse(text)
  } catch {
    return text
  }
}

function getErrorMessage(data: unknown, status: number): string {
  if (typeof data === 'string' && data.trim() !== '') {
    return data
  }

  if (data !== null && typeof data === 'object') {
    const error = data as Record<string, unknown>
    const detail = getNonEmptyString(error.detail)
    const message = getNonEmptyString(error.message)
    const title = getNonEmptyString(error.title)
    const validationErrors = getValidationErrors(error.errors)

    if (detail) {
      return detail
    }
    if (message) {
      return message
    }
    if (title && validationErrors) {
      return `${title}: ${validationErrors}`
    }
    if (title) {
      return title
    }
    if (validationErrors) {
      return validationErrors
    }

    const serialized = JSON.stringify(data)
    if (serialized !== '{}') {
      return serialized
    }
  }

  return `Request failed with status ${status}`
}

function getNonEmptyString(value: unknown): string | undefined {
  return typeof value === 'string' && value.trim() !== ''
    ? value
    : undefined
}

function getValidationErrors(value: unknown): string | undefined {
  if (value === null || typeof value !== 'object') {
    return undefined
  }

  const messages = Object.values(value as Record<string, unknown>)
    .flatMap(item => Array.isArray(item) ? item : [item])
    .filter((item): item is string => typeof item === 'string' && item.trim() !== '')

  const uniqueMessages = Array.from(new Set(messages))

  return uniqueMessages.length > 0
    ? uniqueMessages.join(' ')
    : undefined
}
