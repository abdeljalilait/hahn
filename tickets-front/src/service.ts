import axios from "axios";
import type { Ticket, TicketsResponse } from "./types";

/**
 * Creates an Axios instance with a base URL for the API.
 * This instance can be used to make HTTP requests to the API.
 */
const instance = axios.create({
  baseURL: "http://localhost:5289/api",
});

/**
 * Fetches a page of tickets from the server.
 *
 * @param pageNumber - The page number to fetch.
 * @param pageSize - The number of tickets to fetch per page.
 * @returns A Promise that resolves to the fetched page of tickets.
 */
export const fetchTickets = async ({
  pageNumber,
  pageSize,
}: {
  pageNumber: number;
  pageSize: number;
}): Promise<TicketsResponse> => {
  const { data } = await instance.get(
    `/tickets?pageNumber=${pageNumber}&pageSize=${pageSize}`
  );
  return data;
};

/**
 * Adds a new ticket to the system.
 *
 * @param ticket - The ticket object to add, excluding the `id` property.
 * @returns A Promise that resolves when the ticket has been added.
 */
export const addTicket = async (ticket: Omit<Ticket, "id">) => {
  return await instance.post("/tickets", ticket);
};

/**
 * Updates an existing ticket with the specified ID.
 *
 * @param ticket - The updated ticket object.
 * @returns A Promise that resolves when the ticket has been updated.
 */
export const updateTicket = async (ticket: Ticket) => {
  await instance.put(`/tickets/${ticket.id}`, ticket);
};

/**
 * Deletes a ticket with the specified ID.
 *
 * @param id - The ID of the ticket to delete.
 * @returns A Promise that resolves when the ticket has been deleted.
 */
export const deleteTicket = async (id: number) => {
  await instance.delete(`/tickets/${id}`);
};
