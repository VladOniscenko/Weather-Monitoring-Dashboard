/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateCityRequest } from '../models/CreateCityRequest';
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
     * @returns any OK
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
    ): CancelablePromise<any> {
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
     * @returns any OK
     * @throws ApiError
     */
    public static postApiCities(
        requestBody?: CreateCityRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Cities',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public static getApiCities1(
        id: string,
    ): CancelablePromise<any> {
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
     * @returns any OK
     * @throws ApiError
     */
    public static putApiCities(
        id: string,
        requestBody?: UpdateCityRequest,
    ): CancelablePromise<any> {
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
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiCities(
        id: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Cities/{id}',
            path: {
                'id': id,
            },
        });
    }
}
