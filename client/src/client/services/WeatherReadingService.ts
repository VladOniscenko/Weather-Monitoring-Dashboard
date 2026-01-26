/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
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
     * @returns any OK
     * @throws ApiError
     */
    public static getApiWeatherReading(
        start?: string,
        end?: string,
        stationId?: string,
        page?: number,
        pageSize?: number,
    ): CancelablePromise<any> {
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
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public static getApiWeatherReading1(
        id: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherReading/{id}',
            path: {
                'id': id,
            },
        });
    }
}
