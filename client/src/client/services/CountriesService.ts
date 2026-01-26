/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CountryDtoApiResponse } from '../models/CountryDtoApiResponse';
import type { CountryDtoListApiResponse } from '../models/CountryDtoListApiResponse';
import type { CreateCountryRequest } from '../models/CreateCountryRequest';
import type { ObjectApiResponse } from '../models/ObjectApiResponse';
import type { UpdateCountryRequest } from '../models/UpdateCountryRequest';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class CountriesService {
    /**
     * @returns CountryDtoListApiResponse OK
     * @throws ApiError
     */
    public static getAllCountries(): CancelablePromise<CountryDtoListApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Countries',
        });
    }
    /**
     * @param requestBody
     * @returns CountryDtoApiResponse OK
     * @throws ApiError
     */
    public static createCountry(
        requestBody?: CreateCountryRequest,
    ): CancelablePromise<CountryDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Countries',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns CountryDtoApiResponse OK
     * @throws ApiError
     */
    public static getCountryById(
        id: string,
    ): CancelablePromise<CountryDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Countries/{id}',
            path: {
                'id': id,
            },
        });
    }
    /**
     * @param id
     * @param requestBody
     * @returns CountryDtoApiResponse OK
     * @throws ApiError
     */
    public static updateCountry(
        id: string,
        requestBody?: UpdateCountryRequest,
    ): CancelablePromise<CountryDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Countries/{id}',
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
    public static deleteCountry(
        id: string,
    ): CancelablePromise<ObjectApiResponse> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Countries/{id}',
            path: {
                'id': id,
            },
        });
    }
}
