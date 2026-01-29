/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
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
        pageSize?: number
    ): CancelablePromise<WeatherReadingDtoListApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherReading',
            query: {
                Start: start,
                End: end,
                StationId: stationId,
                Page: page,
                PageSize: pageSize,
            },
        });
    }
    /**
     * @param id
     * @returns WeatherReadingDtoApiResponse OK
     * @throws ApiError
     */
    public static getReadingById(
        id: string
    ): CancelablePromise<WeatherReadingDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherReading/{id}',
            path: {
                id: id,
            },
        });
    }
}
