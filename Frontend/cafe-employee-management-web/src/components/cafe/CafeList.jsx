import { useEffect, useState } from "react";
import { deleteCafe, getCafes } from "../../services/api";
import { AgGridReact } from "ag-grid-react";
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Box, Typography, Button, TextField } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import DeleteDialog from "./../common/DeleteDialog";
import { ToastContainer, toast } from "react-toastify";

export default function CafeList() {

    const navigate = useNavigate();
    const [originalRowData, setOriginalRowData] = useState([]);
    const [rowData, setRowData] = useState(originalRowData);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [itemToDelete, setItemToDelete] = useState(null);
    const [searchTerm, setSearchTerm] = useState('');

    const baseURL = 'https://localhost:7199/';

    const [columnDefs, setColumnDefs] = useState([
        // { field: 'logo', headerName: 'Logo' },
        {
            headerName: "Logo",
            field: "logo",
            cellRenderer: (params) => {
                // const div = document.createElement("div");
                // div.innerHTML = `<img src="${baseURL + params.value}" style="height: 14px; width: 14px;"/>`;
                // return div;

                <img src={baseURL + params.value} style="height: 14px; width: 14px;" />

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
        setSearchTerm(e.target.value);
        console.log(searchTerm);
        if (!searchTerm) {
            setRowData(originalRowData);
        } else {
            const filteredData = originalRowData.filter((row) =>
                row.location.toLowerCase().includes(searchTerm.toLowerCase())
            );

            console.log(searchTerm);
            console.log(filteredData);

            setRowData(filteredData);
        }
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
                setOriginalRowData(data);
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