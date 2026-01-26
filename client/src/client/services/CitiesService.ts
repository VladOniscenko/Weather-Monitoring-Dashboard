/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CityDtoApiResponse } from '../models/CityDtoApiResponse';
import type { CityDtoListApiResponse } from '../models/CityDtoListApiResponse';
import type { CreateCityRequest } from '../models/CreateCityRequest';
import type { ObjectApiResponse } from '../models/ObjectApiResponse';
import type { UpdateCityRequest } from '../models/UpdateCityRequest';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class CitiesService {
    /**
     * @param name
     * @param latitude
     * @param longitude
     * @param lookInsideBounds
     * @param countryId
     * @param page
     * @param pageSize
     * @returns CityDtoListApiResponse OK
     * @throws ApiError
     */
    public static getApiCities(
        name?: string,
        latitude?: string,
        longitude?: string,
        lookInsideBounds?: boolean,
        countryId?: string,
        page?: number,
        pageSize?: number,
    ): CancelablePromise<CityDtoListApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Cities',
            query: {
                'Name': name,
                'Latitude': latitude,
                'Longitude': longitude,
                'LookInsideBounds': lookInsideBounds,
                'CountryId': countryId,
                'Page': page,
                'PageSize': pageSize,
            },
        });
    }
    /**
     * @param requestBody
     * @returns CityDtoApiResponse OK
     * @throws ApiError
     */
    public static postApiCities(
        requestBody?: CreateCityRequest,
    ): CancelablePromise<CityDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Cities',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns CityDtoApiResponse OK
     * @throws ApiError
     */
    public static getApiCities1(
        id: string,
    ): CancelablePromise<CityDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Cities/{id}',
            path: {
                'id': id,
            },
        });
    }
    /**
     * @param id
     * @param requestBody
     * @returns CityDtoApiResponse OK
     * @throws ApiError
     */
    public static putApiCities(
        id: string,
        requestBody?: UpdateCityRequest,
    ): CancelablePromise<CityDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Cities/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns ObjectApiResponse OK
     * @throws ApiError
     */
    public static deleteApiCities(
        id: string,
    ): CancelablePromise<ObjectApiResponse> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Cities/{id}',
            path: {
                'id': id,
            },
        });
    }
}
