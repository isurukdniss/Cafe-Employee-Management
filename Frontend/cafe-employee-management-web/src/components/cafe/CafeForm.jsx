
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { getCafe, createCafe, updateCafe } from "../../services/api";
import { useParams, useNavigate, useBeforeUnload } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import { Box, Typography, Button, TextField } from '@mui/material';
import ReusableTextBox from "../common/ReusableTextBox";

export default function CafeForm() {
    const {
        register,
        handleSubmit,
        reset,
        formState: { errors, isDirty },
        control,
    } = useForm({
        defaultValues: {
            name: '',
            description: '',
            location: '',
            logo: '',
        }
    }
    )

    const [loading, setLoading] = useState(false);
    const [file, setFile] = useState(null);
    const { id } = useParams();
    const navigate = useNavigate();

    useBeforeUnload((event) => {
        if (isDirty) {
            event.preventDefault();
        }
    });

    const loadCafeById = async () => {
        try {
            const response = await getCafe(id);
            const formData = response.data.data;
            reset({
                name: formData.name || '',
                description: formData.description || '',
                logo: formData.logo || '',
                location: formData.location || '',
            })

        } catch (error) {
            console.log('Error loading employee data: ', error);
        }
    }

    useEffect(() => {
        if (id) {
            loadCafeById(id);
        }
    }, []);

    const handleFileChange = (e) => {
        setFile(e.target.files[0]);
    };

    const onSubmit = async (data) => {
        console.log(data)
        try {
            const formData = new FormData();
            formData.append("name", data.name);
            formData.append("description", data.description);
            formData.append("location", data.location);
            formData.append("logoFile", file);

            if (id) {
                data.id = id;
                const response = await updateCafe(id, formData);
                if (response && response.data && response.data.success) {
                    toast.success('Successfully updated the employee.');
                    setTimeout(() => {
                        navigate('/');
                    }, 1000);

                    reset();
                }
            } else {
                const response = await createCafe(formData);
                console.log(response);
                if (response && response.data && response.data.success) {
                    toast.success('Successfully created the cafe.');
                    setTimeout(() => {
                        navigate('/');
                    }, 1000);
                    reset();
                }
            }
        } catch (err) {
            toast.error('Error creating cafe. Please try again.');
        }
    };

    const handleCancel = () => {
        console.log(isDirty);
        if (!isDirty || window.confirm('You have unsaved changes. Do you really want to leave?')) {
            navigate('/');
        }
    }

    return (
        <Box
            sx={{
                maxWidth: 400,
                margin: 'auto',
                padding: 3,
                boxShadow: 3,
                borderRadius: 2,
                backgroundColor: 'white',
            }}
        >
            <Typography variant="h4" gutterBottom>
                {id ? 'Edit Cafe' : 'Add Cafe'}
            </Typography>
            <form onSubmit={handleSubmit(onSubmit)}>
                <ReusableTextBox
                    name="name"
                    label="Name"
                    control={control}
                    rules={{
                        required: 'Name is required',
                        minLength: {
                            value: 6,
                            message: 'Name must be at least 3 characters',
                        },
                        maxLength: {
                            value: 10,
                            message: 'Name cannot exceed 10 characters',
                        },
                    }}
                />
                <ReusableTextBox
                    name="description"
                    label="Description"
                    control={control}
                    rules={{
                        required: 'Description is required',
                        maxLength: {
                            value: 256,
                            message: 'Description cannot exceed 256 characters',
                        },
                    }}
                />
                <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', padding: 2 }}>
                    <Typography variant="body1" gutterBottom>
                        Upload a Logo
                    </Typography>
                    <TextField
                        variant="outlined"
                        fullWidth
                        type="file"
                        onChange={handleFileChange}
                        sx={{ marginBottom: 2 }}
                    />
                </Box>
                <ReusableTextBox
                    name="location"
                    label="Location"
                    control={control}
                    rules={{
                        required: 'Location is required',
                        maxLength: {
                            value: 50,
                            message: 'Location cannot exceed 50 characters',
                        },
                    }}
                />
                <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: 2 }}>
                    <Button
                        variant="contained"
                        color="primary"
                        type="submit"
                        sx={{ flex: 1, marginRight: 1 }}
                    >
                        Submit
                    </Button>
                    <Button
                        variant="outlined"
                        color="secondary"
                        onClick={handleCancel}
                        sx={{ flex: 1, marginLeft: 1 }}
                    >
                        Cancel
                    </Button>
                </Box>
            </form>
            <ToastContainer />
        </Box>
    );
}