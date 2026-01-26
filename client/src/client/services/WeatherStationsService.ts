/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateWeatherStationRequest } from '../models/CreateWeatherStationRequest';
import type { UpdateWeatherStationRequest } from '../models/UpdateWeatherStationRequest';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class WeatherStationsService {
    /**
     * @param cityId
     * @param name
     * @param page
     * @param pageSize
     * @returns any OK
     * @throws ApiError
     */
    public static getApiWeatherStations(
        cityId?: string,
        name?: string,
        page?: number,
        pageSize?: number,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherStations',
            query: {
                'CityId': cityId,
                'Name': name,
                'Page': page,
                'PageSize': pageSize,
            },
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static postApiWeatherStations(
        requestBody?: CreateWeatherStationRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/WeatherStations',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public static getApiWeatherStations1(
        id: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherStations/{id}',
            path: {
                'id': id,
            },
        });
    }
    /**
     * @param id
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static putApiWeatherStations(
        id: string,
        requestBody?: UpdateWeatherStationRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/WeatherStations/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiWeatherStations(
        id: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/WeatherStations/{id}',
            path: {
                'id': id,
            },
        });
    }
}
