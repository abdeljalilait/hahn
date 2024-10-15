import axios from "axios";
import type { Ticket, TicketsResponse } from "./types";

const instance = axios.create({
  baseURL: "http://localhost:5289/api",
});
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

export const addTicket = async (ticket: Omit<Ticket, "id">) => {
  return await instance.post("/tickets", ticket);
};

export const updateTicket = async (ticket: Ticket) => {
  await instance.put(`/tickets/${ticket.id}`, ticket);
};

export const deleteTicket = async (id: number) => {
  await instance.delete(`/tickets/${id}`);
};
