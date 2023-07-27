import axios, { type AxiosRequestConfig, type RawAxiosRequestHeaders } from "axios";
import auth from "../auth-service";

const baseUrl = 'https://localhost:32678/api';

async function getHeaders(authenticated: boolean): Promise<AxiosRequestConfig> {
  if (!authenticated) {
    return {};
  }

  return {
    withCredentials: true,
    headers: {
      Authorization: `Bearer ${await auth.getToken()}`
    }
  };
}

export async function httpget<T>(endpoint: string, authenticated = true): Promise<T> {
  const response = await axios.get<T>(`${baseUrl}/${endpoint}`, 
    await getHeaders(authenticated));

  return response.data;
}

export async function httpdelete<T>(endpoint: string, authenticated = true): Promise<T> {
  const response = await axios.delete<T>(`${baseUrl}/${endpoint}`, 
    await getHeaders(authenticated));

  return response.data;
}

export async function httppost<T>(endpoint: string, body: object, authenticated = true): Promise<T> {
  const response = await axios.post<T>(`${baseUrl}/${endpoint}`, body, 
    await getHeaders(authenticated));

  return response.data;
}