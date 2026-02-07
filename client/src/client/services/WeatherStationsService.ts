/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateWeatherStationRequest } from '../models/CreateWeatherStationRequest';
import type { StationCordinateDtoPagedResponseApiResponse } from '../models/StationCordinateDtoPagedResponseApiResponse';
import type { UpdateWeatherStationRequest } from '../models/UpdateWeatherStationRequest';
import type { WeatherStationDtoApiResponse } from '../models/WeatherStationDtoApiResponse';
import type { WeatherStationDtoPagedResponseApiResponse } from '../models/WeatherStationDtoPagedResponseApiResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class WeatherStationsService {
    /**
     * @param cityId
     * @param name
     * @param minLng
     * @param maxLng
     * @param minLat
     * @param maxLat
     * @param zoom
     * @param userId
     * @param page
     * @param pageSize
     * @returns WeatherStationDtoPagedResponseApiResponse OK
     * @throws ApiError
     */
    public static getAllStations(
        cityId?: string,
        name?: string,
        minLng?: number,
        maxLng?: number,
        minLat?: number,
        maxLat?: number,
        zoom?: number,
        userId?: string,
        page?: number,
        pageSize?: number,
    ): CancelablePromise<WeatherStationDtoPagedResponseApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/WeatherStations',
            query: {
                'CityId': cityId,
                'Name': name,
                'MinLng': minLng,
                'MaxLng': maxLng,
                'MinLat': minLat,
                'MaxLat': maxLat,
                'Zoom': zoom,
                'UserId': userId,
                'Page': page,
                'PageSize': pageSize,
            },
        });
    }
    /**
     * @param requestBody
     * @returns WeatherStationDtoApiResponse OK
     * @throws ApiError
     */
    public static createStation(
        requestBody?: CreateWeatherStationRequest,
    ): CancelablePromise<WeatherStationDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/WeatherStations',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param cityId
     * @param name
     * @param minLng
     * @param maxLng
     * @param minLat
     * @param maxLat
     * @param zoom
     * @param userId
     * @param page
     * @param pageSize
     * @returns StationCordinateDtoPagedResponseApiResponse OK
     * @throws ApiError
     */
    public static getAllStationsCordinates(
        cityId?: string,
        name?: string,
        minLng?: number,
        maxLng?: number,
        minLat?: number,
        maxLat?: number,
        zoom?: number,
        userId?: string,
        page?: number,
        pageSize?: number,
    ): CancelablePromise<StationCordinateDtoPagedResponseApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/cordinates',
            query: {
                'CityId': cityId,
                'Name': name,
                'MinLng': minLng,
                'MaxLng': maxLng,
                'MinLat': minLat,
                'MaxLat': maxLat,
                'Zoom': zoom,
                'UserId': userId,
                'Page': page,
                'PageSize': pageSize,
            },
        });
    }
    /**
     * @param id
     * @returns WeatherStationDtoApiResponse OK
     * @throws ApiError
     */
    public static getApiWeatherStations(
        id: string,
    ): CancelablePromise<WeatherStationDtoApiResponse> {
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
    public static updateStation(
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
    public static deleteStation(
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
