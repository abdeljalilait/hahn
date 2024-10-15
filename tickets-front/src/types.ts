/**
 * Represents a single ticket, containing an ID, description, and status.
 */
export interface Ticket {
  id: number;
  description: string;
  status: "Open" | "Closed";
}

/**
 * Represents the response from the tickets API, containing a list of tickets and pagination information.
 */
export interface TicketsResponse {
  data: Ticket[];
  pagination: {
    totalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
  };
}
