
import { set, useForm, Controller } from "react-hook-form";
import { useEffect, useState } from "react";
import { getCafes, getEmployee, createEmployee, updateEmployee } from "../../services/api";
import { useParams, useNavigate, useBeforeUnload } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import ReusableTextBox from "../common/ReusableTextBox";
import { Box, Typography, Button } from '@mui/material';

import '../common/Common.css'

export default function EmployeeForm() {
    const {
        register,
        handleSubmit,
        reset,
        control,
        setValue,
        formState: { errors, isDirty },
    } = useForm()

    const [cafes, setCafes] = useState([]);
    const [loading, setLoading] = useState(true);
    const { id } = useParams();
    const navigate = useNavigate();

    useBeforeUnload((event) => {
        if (isDirty) {
            event.preventDefault();
        }
    });

    const loadCafes = async () => {
        try {
            const response = await getCafes();
            const { success, data, errors } = response.data;

            if (success) {
                setCafes(data);
            } else {
                setError(errors.length > 0 ? errors.join(', ') : "Unknown error occurred");
            }
        } catch (err) {
            setError(err.message || 'Failed to fetch data');
        } finally {
            setLoading(false);
        }
    }

    const loadEmployeeById = async () => {
        try {
            const response = await getEmployee(id);
            const formData = response.data.data;

            console.log(formData)
            console.log("Gender value in formData:", formData.gender);

            reset({
                name: formData.name || '',
                email: formData.email || '',
                phoneNumber: formData.phoneNumber || '',
                gender: formData.gender.toString() || '',
                cafeId: formData.cafeId || '',
            })

        } catch (error) {
            console.log('Error loading employee data: ', error);
        }
    }

    useEffect(() => {
        if (id) {
            loadEmployeeById(id);
        }

        loadCafes();
    }, []);

    const onSubmit = async (data) => {
        console.log(data);

        const formattedData = {
            ...data,
            phoneNumber: Number(data.phoneNumber),
            gender: Number(data.gender),
        };
        try {
            if (id) {
                formattedData.id = id;
                const response = await updateEmployee(id, formattedData);
                if (response && response.data && response.data.success) {
                    toast.success('Successfully updated the employee.');
                    console.log('Employee updated');
                    setTimeout(() => {
                        navigate('/employees');
                    }, 1000);

                    reset();
                }
            } else {
                const response = await createEmployee(formattedData);
                if (response && response.data && response.data.success) {
                    toast.success('Successfully created the employee.');
                    console.log(`Employee created. Id = ${response.data.data}`);
                    navigate('/employees');

                    reset();
                }
            }
        } catch (err) {
            console.error('Error creating employee: ' + err);
            toast.error('Error creating employee. Please try again.');
        }
    };

    const handleCancel = () => {
        console.log(isDirty);
        if (!isDirty || window.confirm('You have unsaved changes. Do you really want to leave?')) {
            navigate('/employees');
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
                marginTop: 5,
            }}
        >
            <Typography variant="h4" gutterBottom>
                {id ? 'Edit Employee' : 'Add Employee'}
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
                            message: 'Name must be at least 6 characters',
                        },
                        maxLength: {
                            value: 10,
                            message: 'Name cannot exceed 10 characters',
                        },
                    }}
                />
                <ReusableTextBox
                    name="email"
                    label="Email"
                    type="email"
                    control={control}
                    rules={{
                        required: 'Email is required',
                        pattern: {
                            value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                            message: 'Invalid email address',
                        },
                    }}
                />
                <ReusableTextBox
                    name="phoneNumber"
                    label="Phone Number"
                    control={control}
                    rules={{
                        required: 'Phone number is required',
                        pattern: {
                            value: /^[89][0-9]{7}$/,
                            message: 'Phone number must start with 8 or 9 and have 8 digits',
                        },
                    }}
                />
                <div className="radio-group-container">
                    <label className="radio-group-label">Gender</label>
                    <div className="radio-group-option">
                        <input type="radio" name="male" value="1" {...register('gender')} />
                        <label htmlFor="male">Male</label>
                    </div>
                    <div className="radio-group-option">
                        <input type="radio" name="female" value="2" {...register('gender')} />
                        <label htmlFor="female">Female</label>
                    </div>
                    {errors.gender && (
                        <span className="error-message">{errors.gender.message}</span>
                    )}
                </div>
                <div className="dropdown-container">
                    <label className="dropdown-label" htmlFor="cafe">Assigned Cafe</label>
                    {loading ? (
                        <p className="loading-text">Loading Cafes...</p>
                    ) : (
                        <select
                            {...register('cafeId', { required: 'Please select a cafe' })}
                            className="dropdown-select"
                        >
                            <option selected="selected" value="" disabled>-- Select a Cafe --</option>
                            {cafes.map((cafe) => (
                                <option key={cafe.id} value={cafe.id}>{cafe.name}</option>
                            ))}
                        </select>
                    )}
                    {errors.cafe && (
                        <span className="error-message">{errors.cafe.message}</span>
                    )}
                </div>
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