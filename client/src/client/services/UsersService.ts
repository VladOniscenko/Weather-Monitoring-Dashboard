/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { UserDtoApiResponse } from '../models/UserDtoApiResponse';
import type { UserLoginRequestDto } from '../models/UserLoginRequestDto';
import type { UserLoginResponseDtoApiResponse } from '../models/UserLoginResponseDtoApiResponse';
import type { UserRegisterRequestDto } from '../models/UserRegisterRequestDto';
import type { UserRegisterResponseDtoApiResponse } from '../models/UserRegisterResponseDtoApiResponse';
import type { UserUpdateRequestDto } from '../models/UserUpdateRequestDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class UsersService {
    /**
     * @param requestBody
     * @returns UserRegisterResponseDtoApiResponse OK
     * @throws ApiError
     */
    public static registerUser(
        requestBody?: UserRegisterRequestDto,
    ): CancelablePromise<UserRegisterResponseDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/register',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns UserLoginResponseDtoApiResponse OK
     * @throws ApiError
     */
    public static loginUser(
        requestBody?: UserLoginRequestDto,
    ): CancelablePromise<UserLoginResponseDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/login',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns UserDtoApiResponse OK
     * @throws ApiError
     */
    public static getCurrentUser(): CancelablePromise<UserDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/me',
        });
    }
    /**
     * @param requestBody
     * @returns UserDtoApiResponse OK
     * @throws ApiError
     */
    public static patchProfile(
        requestBody?: UserUpdateRequestDto,
    ): CancelablePromise<UserDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/profile',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns UserDtoApiResponse OK
     * @throws ApiError
     */
    public static getUserById(
        id: string,
    ): CancelablePromise<UserDtoApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/{Id}',
            path: {
                'Id': id,
            },
        });
    }
}
