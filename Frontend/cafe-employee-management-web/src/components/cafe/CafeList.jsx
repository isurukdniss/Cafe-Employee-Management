import { useEffect, useState } from "react";
import { deleteCafe, getCafes, BASE_URL } from "../../services/api";
import { AgGridReact } from "ag-grid-react";
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Box, Typography, Button, TextField } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import DeleteDialog from "./../common/DeleteDialog";
import { ToastContainer, toast } from "react-toastify";

export default function CafeList() {

    const navigate = useNavigate();
    const [rowData, setRowData] = useState([]);
    const [filteredData, setFilteredData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [itemToDelete, setItemToDelete] = useState(null);
    const [searchTerm, setSearchTerm] = useState('');

    const [columnDefs, setColumnDefs] = useState([
        {
            headerName: "Logo",
            field: "logo",
            cellRenderer: (params) => {
                return (
                    <img
                        src={BASE_URL + params.value}
                        alt="Cafe Logo"
                        style={{ width: "20px", height: "20px", objectFit: "cover" }}
                    />
                )

            },
        },
        { field: 'name', headerName: 'Name' },
        { field: 'description', headerName: 'Description' },
        { field: 'employees', headerName: 'Employees' },
        { field: 'location', headerName: 'Location' },
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

    const handleAddClick = () => {
        navigate('/cafes/add');
    }

    const handleEdit = (rowData) => {
        navigate(`/cafes/edit/${rowData.id}`);
    };

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
    };

    const handleOpenDialog = (id) => {
        setItemToDelete(id);
        setIsDialogOpen(true);
    };

    const handleDelete = async () => {
        setIsDialogOpen(false);
        try {
            console.log(itemToDelete);
            const response = await deleteCafe(itemToDelete.id)
            const { success, data, errors } = response.data;
            if (success) {
                toast.success("Cafe deleted successfully");
                loadCafes();
            }
        } catch (error) {
            toast.error("Faild to delete cafe");
        }
    };

    const handleSearch = (e) => {
        const value = e.target.value.toLowerCase();
        setSearchTerm(value);

        const filtered = rowData.filter((cafe) =>
            cafe.location.toLowerCase().includes(value)
        );
        setFilteredData(filtered);
    }

    useEffect(() => {
        loadCafes();
    }, []);

    const loadCafes = async () => {
        try {
            const response = await getCafes();
            const { success, data, errors } = response.data;

            if (success) {
                setRowData(data);
                setFilteredData(data);
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

    return (
        <>
            <Box sx={{ padding: 3 }}>
                <Typography variant="h4" gutterBottom>
                    Cafes
                </Typography>
                <Box sx={{ marginBottom: 2 }}>
                    <TextField
                        label="Search by Location"
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
                        Add New Cafe
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
                        rowData={filteredData}
                        columnDefs={columnDefs}
                        defaultColDef={{
                            sortable: true,
                            filter: true,
                            resizable: true,
                        }}
                        pagination={true}
                        paginationPageSize={10}
                    />
                </Box>
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