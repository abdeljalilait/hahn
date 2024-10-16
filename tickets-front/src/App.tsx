import { useState } from "react";
import {
  Table,
  Button,
  Input,
  Form,
  Select,
  Space,
  message,
  Popconfirm,
} from "antd";
import { useQuery, useMutation, useQueryClient } from "react-query";
import type { Ticket, TicketsResponse } from "./types"; // Adjust the path as necessary
import { addTicket, deleteTicket, fetchTickets, updateTicket } from "./service";
import dayjs from "dayjs";
import type { ColumnType } from "antd/es/table";

const { Option } = Select;

const App: React.FC = () => {
  const [editingTicket, setEditingTicket] = useState<Ticket | null>(null);
  const [pagination, setPagination] = useState({ pageNumber: 1, pageSize: 10 });
  const [newTicket, setNewTicket] = useState<Omit<Ticket, "id">>({
    description: "",
    status: "Open",
  });

  const queryClient = useQueryClient();

  const {
    data: tickets,
    isLoading,
    isError,
  } = useQuery<TicketsResponse, Error>(["tickets", pagination], () =>
    fetchTickets(pagination)
  );

  const addTicketMutation = useMutation(addTicket, {
    onSuccess: () => {
      queryClient.invalidateQueries("tickets");
      message.success("New ticket added successfully");
      setNewTicket({ description: "", status: "Open" });
    },

    onError: (error) => {
      message.error("Failed to add ticket");
      console.error(error);
    },
  });

  const updateTicketMutation = useMutation(updateTicket, {
    onSuccess: () => {
      queryClient.invalidateQueries("tickets");
      message.success("Ticket updated successfully");
      setEditingTicket(null);
    },

    onError: (error) => {
      message.error("Failed to update ticket");
      console.error(error);
    },
  });

  const deleteTicketMutation = useMutation(deleteTicket, {
    onSuccess: () => {
      queryClient.invalidateQueries("tickets");
      message.success("Ticket deleted successfully");
    },

    onError: (error) => {
      message.error("Failed to delete ticket");
      console.error(error);
    },
  });

  const handleUpdate = (ticket: Ticket) => {
    setEditingTicket(ticket);
  };

  const handleDelete = (id: number) => {
    deleteTicketMutation.mutate(id);
  };

  const handleSaveUpdate = () => {
    if (editingTicket) {
      updateTicketMutation.mutate(editingTicket);
    }
  };

  const handleAddTicket = () => {
    addTicketMutation.mutate(newTicket);
  };

  /**
   * Defines the columns for the ticket table, including rendering custom components for editable fields.
   * The columns include:
   * - Ticket Id: Sortable by ticket ID
   * - Description: Editable via an input field when the ticket is being edited
   * - Status: Editable via a dropdown when the ticket is being edited
   * - Date: Formatted date of ticket creation
   * - Actions: Provides buttons to update or delete the ticket
   */
  const columns: ColumnType<Ticket>[] = [
    {
      title: "Ticket Id",
      dataIndex: "id",
      key: "id",
      sorter: (a, b) => a.id - b.id,
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
      render: (text: string, record: Ticket) =>
        editingTicket?.id === record.id ? (
          <Input
            required
            value={editingTicket.description}
            onChange={(e) =>
              setEditingTicket({
                ...editingTicket,
                description: e.target.value,
              })
            }
          />
        ) : (
          text
        ),
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      render: (text: string, record: Ticket) =>
        editingTicket?.id === record.id ? (
          <Select
            value={editingTicket.status}
            onChange={(value) =>
              setEditingTicket({ ...editingTicket, status: value })
            }
          >
            <Option value="Open">Open</Option>
            <Option value="Closed">Closed</Option>
          </Select>
        ) : (
          text
        ),
    },
    {
      title: "Date",
      dataIndex: "createdAt",
      key: "createdAt",
      render(date: string) {
        return dayjs(date).format("MMM-DD-YYYY");
      },
    },
    {
      title: "Actions",
      key: "actions",
      render: (_: string, record: Ticket) =>
        editingTicket?.id === record.id ? (
          <Space>
            <Button type="link" onClick={handleSaveUpdate}>
              Save
            </Button>
            <Button type="link" onClick={() => setEditingTicket(null)}>
              Cancel
            </Button>
          </Space>
        ) : (
          <Space>
            <Button type="link" onClick={() => handleUpdate(record)}>
              Update
            </Button>
            <Popconfirm
              title="Are you sure to delete this ticket?"
              onConfirm={() => handleDelete(record.id)}
            >
              <Button type="link" danger>
                Delete
              </Button>
            </Popconfirm>
          </Space>
        ),
    },
  ];

  if (isError) return <div>Error loading tickets</div>;

  return (
    <div>
      <Table
        loading={isLoading}
        columns={columns}
        dataSource={tickets?.data}
        rowKey="id"
        pagination={{
          current: pagination.pageNumber,
          pageSize: pagination.pageSize,
          total: tickets?.pagination.totalCount, // Total count for pagination
          onChange: (page, pageSize) => {
            setPagination({
              pageNumber: page,
              pageSize: pageSize,
            });
          },
        }}
        footer={() => (
          <Form layout="inline" onFinish={handleAddTicket}>
            <Form.Item>
              <Input
                placeholder="Enter description"
                value={newTicket.description}
                onChange={(e) =>
                  setNewTicket({ ...newTicket, description: e.target.value })
                }
                required
              />
            </Form.Item>
            <Form.Item>
              <Select
                value={newTicket.status}
                onChange={(value) =>
                  setNewTicket({ ...newTicket, status: value })
                }
              >
                <Option value="Open">Open</Option>
                <Option value="Closed">Closed</Option>
              </Select>
            </Form.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit">
                Add Ticket
              </Button>
            </Form.Item>
          </Form>
        )}
      />
    </div>
  );
};
export default App;
