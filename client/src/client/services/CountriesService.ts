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
     * @param name
     * @param cca2
     * @param cca3
     * @param region
     * @param subregion
     * @param capital
     * @param independent
     * @param landlocked
     * @param latitude
     * @param longitude
     * @param lookInsideBounds
     * @param page
     * @param pageSize
     * @returns CountryDtoListApiResponse OK
     * @throws ApiError
     */
    public static getAllCountries(
        name?: string,
        cca2?: string,
        cca3?: string,
        region?: string,
        subregion?: string,
        capital?: string,
        independent?: boolean,
        landlocked?: boolean,
        latitude?: string,
        longitude?: string,
        lookInsideBounds?: boolean,
        page?: number,
        pageSize?: number,
    ): CancelablePromise<CountryDtoListApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Countries',
            query: {
                'Name': name,
                'CCA2': cca2,
                'CCA3': cca3,
                'Region': region,
                'Subregion': subregion,
                'Capital': capital,
                'Independent': independent,
                'Landlocked': landlocked,
                'Latitude': latitude,
                'Longitude': longitude,
                'LookInsideBounds': lookInsideBounds,
                'Page': page,
                'PageSize': pageSize,
            },
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
