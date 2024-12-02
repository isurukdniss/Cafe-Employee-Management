import { useEffect, useState } from "react";
import { getEmployees, deleteEmployee } from "../../services/api";
import { AgGridReact } from "ag-grid-react";
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { useNavigate } from 'react-router-dom'
import { ToastContainer, toast } from "react-toastify";
import { Box, Typography, Button, TextField } from '@mui/material';
import DeleteDialog from "./../common/DeleteDialog";


export default function EmployeeList() {
    const [rowData, setRowData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [itemToDelete, setItemToDelete] = useState(null);
    const [error, setError] = useState(null);
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();
    const [columnDefs, setColumnDefs] = useState([
        { field: 'id', headerName: 'ID' },
        { field: 'name', headerName: 'Name' },
        { field: 'email', headerName: 'Email' },
        { field: 'phoneNumber', headerName: 'Phone Number' },
        {
            field: 'gender', headerName: 'Gender',
            valueFormatter: (params) => (params.value === 1 ?
                'Male' : params.value === 2 ? 'Female' : 'Unknown'),
        },
        { field: 'daysWorked', headerName: 'Days Worked in the café' },
        { field: 'cafeName', headerName: 'Café Name' },
        {
            headerName: 'Actions',
            field: 'actions',
            cellRenderer: (params) => (
                <div>
                    <Button
                        onClick={() => handleEdit(params.data)}
                        variant="outlined"
                        color="primary"
                        size="small"
                        style={{ marginRight: '5px' }}
                    >
                        Edit
                    </Button>
                    <Button
                        onClick={() => handleOpenDialog(params.data)}
                        variant="outlined"
                        color="error"
                        size="small"
                    >
                        Delete
                    </Button>
                </div>
            ),
            sortable: false,
            filter: false,
        },
    ]);


    const handleOpenDialog = (id) => {
        setItemToDelete(id);
        setIsDialogOpen(true);
    };

    const handleEdit = (rowData) => {
        navigate(`/employees/edit/${rowData.id}`);
    };

    const handleDelete = async () => {
        setIsDialogOpen(false);
        try {
            console.log(itemToDelete);
            const response = await deleteEmployee(itemToDelete.id)
            const { success, data, errors } = response.data;
            if (success) {
                toast.success("Employee deleted successfully");
                fetchEmployees();
            }
        } catch (error) {
            toast.error("Faild to delete employee");
        }
    };

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
    };

    const handleAddClick = () => {
        navigate('/employees/add');
    }

    useEffect(() => {
        fetchEmployees();
    }, []);

    const fetchEmployees = async () => {
        try {
            const response = await getEmployees();
            const { success, data, errors } = response.data;

            if (success) {
                setRowData(data);
            } else {
                setError(errors.length > 0 ? errors.join(', ') : "Unknown error occurred");
            }
        } catch (err) {
            setError(err.message || 'Failed to fetch data');
        } finally {
            setLoading(false);
        }
    }

    if (loading) {
        return <p>Loading...</p>
    }

    if (error) {
        return <p>Error: {error}</p>
    }

    const handleSearch = (e) => {
        console.log("Search Term: " + e.target.value);
        setSearchTerm(e.target.value);
        const filteredData = rowData.filter((row) =>
            row.cafeName.toLowerCase().includes(e.target.value.toLowerCase())
        );
        setRowData(filteredData);
    };

    return (
        <>
            <Box sx={{ padding: 3 }}>
                <Typography variant="h4" gutterBottom>
                    Employees
                </Typography>
                <Box sx={{ display: 'flex', alignItems: 'center', marginBottom: 2 }}>
                    <TextField
                        label="Search by Cafe"
                        variant="outlined"
                        size="small"
                        sx={{ marginRight: 2 }}
                        value={searchTerm}
                        onChange={handleSearch}
                    />
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={handleAddClick}
                    >
                        Add New Employee
                    </Button>
                </Box>
                <Box
                    sx={{
                        width: '100%',
                        height: '400px',
                        '& .ag-theme-alpine': {
                            height: '100%',
                            width: '100%',
                        },
                    }}
                    className="ag-theme-alpine"
                >
                    <AgGridReact
                        rowData={rowData}
                        columnDefs={columnDefs}
                        defaultColDef={{
                            sortable: true,
                            filter: true,
                            resizable: true,
                        }}
                        pagination={true}
                        paginationPageSize={10}
                    />
                </Box>s
            </Box>
            <DeleteDialog
                isDialogOpen={isDialogOpen}
                handleCloseDialog={handleCloseDialog}
                handleDelete={handleDelete}
            />
            <ToastContainer />
        </>
    );
}


