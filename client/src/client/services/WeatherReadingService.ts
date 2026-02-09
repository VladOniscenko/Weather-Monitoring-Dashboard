/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateWeatherReadingRequest } from '../models/CreateWeatherReadingRequest';
import type { WeatherReadingDtoApiResponse } from '../models/WeatherReadingDtoApiResponse';
import type { WeatherReadingDtoListApiResponse } from '../models/WeatherReadingDtoListApiResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class WeatherReadingService {
    /**
     * @param start
     * @param end
     * @param stationId
     * @param page
     * @param pageSize
     * @returns WeatherReadingDtoListApiResponse OK
     * @throws ApiError
     */
    public static getReadings(
        start?: string,
        end?: string,
        stationId?: string,
        page?: number,
        pageSize?: number,
    ): CancelablePromise<WeatherReadingDtoListApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherReading',
            query: {
                'Start': start,
                'End': end,
                'StationId': stationId,
                'Page': page,
                'PageSize': pageSize,
            },
        });
    }
    /**
     * @param requestBody
     * @returns WeatherReadingDtoApiResponse OK
     * @throws ApiError
     */
    public static createReading(
        requestBody?: CreateWeatherReadingRequest,
    ): CancelablePromise<WeatherReadingDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/WeatherReading',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns WeatherReadingDtoApiResponse OK
     * @throws ApiError
     */
    public static getReadingById(
        id: string,
    ): CancelablePromise<WeatherReadingDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherReading/{id}',
            path: {
                'id': id,
            },
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public static deleteReading(
        id: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/WeatherReading/{id}',
            path: {
                'id': id,
            },
        });
    }
}
