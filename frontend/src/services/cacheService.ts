import { Activity, ActivityDetail } from "../interfaces/Activity";

const CACHE_KEY_ACTIVITIES = 'cachedActivities';
const CACHE_KEY_ACTIVITY_DETAILS = 'cachedActivityDetails';
const CACHE_KEY_IMAGES = 'cachedActivityImages';
const CACHE_KEY_FACILITY_TYPES = 'cachedFacilityTypes';
const CACHE_KEY_ACTIVITY_TYPES = 'cachedActivityTypes';
const CACHE_KEY_EDUCATORS = 'cachedEducators';
const CACHE_KEY_TOP_ACTIVITIES = 'cachedTopActivities';

export const cacheService = {
  saveActivities: (activities: Activity[]) => {
    localStorage.setItem(CACHE_KEY_ACTIVITIES, JSON.stringify(activities));
  },

  loadActivities: () => {
    const cachedActivities = localStorage.getItem(CACHE_KEY_ACTIVITIES);
    return cachedActivities ? JSON.parse(cachedActivities) : [];
  },

  saveActivityDetails: (activityDetails: ActivityDetail[]) => {
    localStorage.setItem(CACHE_KEY_ACTIVITY_DETAILS, JSON.stringify(activityDetails));
  },

  loadActivityDetails: () => {
    const cachedActivityDetails = localStorage.getItem(CACHE_KEY_ACTIVITY_DETAILS);
    return cachedActivityDetails ? JSON.parse(cachedActivityDetails) : [];
  },

  saveImages: (images: { [id: string]: string }) => {
    localStorage.setItem(CACHE_KEY_IMAGES, JSON.stringify(images));
  },

  loadImages: () => {
    const cachedImages = localStorage.getItem(CACHE_KEY_IMAGES);
    return cachedImages ? JSON.parse(cachedImages) : {};
  },

  saveFacilityTypes: (facilityTypes: string[]) => {
    localStorage.setItem(CACHE_KEY_FACILITY_TYPES, JSON.stringify(facilityTypes));
  },

  loadFacilityTypes: () => {
    const cachedFacilityTypes = localStorage.getItem(CACHE_KEY_FACILITY_TYPES);
    return cachedFacilityTypes ? JSON.parse(cachedFacilityTypes) : [];
  },

  saveActivityTypes: (activityTypes: string[]) => {
    localStorage.setItem(CACHE_KEY_ACTIVITY_TYPES, JSON.stringify(activityTypes));
  },

  loadActivityTypes: () => {
    const cachedActivityTypes = localStorage.getItem(CACHE_KEY_ACTIVITY_TYPES);
    return cachedActivityTypes ? JSON.parse(cachedActivityTypes) : [];
  },

  saveEducators: (educators: { id: string, displayName: string }[]) => {
    localStorage.setItem(CACHE_KEY_EDUCATORS, JSON.stringify(educators));
  },

  loadEducators: () => {
    const cachedEducators = localStorage.getItem(CACHE_KEY_EDUCATORS);
    return cachedEducators ? JSON.parse(cachedEducators) : [];
  },

  saveActivityDetail: (activityDetail: ActivityDetail) => {
    const activityDetails = cacheService.loadActivityDetails();
    const updatedActivityDetails = activityDetails.filter((a: ActivityDetail) => a.id !== activityDetail.id);
    updatedActivityDetails.push(activityDetail);
    cacheService.saveActivityDetails(updatedActivityDetails);
  },

  loadActivityDetail: (id: string) => {
    const activityDetails = cacheService.loadActivityDetails();
    return activityDetails.find((activityDetail: ActivityDetail) => activityDetail.id === id) || null;
  },

  saveTopActivities: (topActivities: Activity[]) => {
    localStorage.setItem(CACHE_KEY_TOP_ACTIVITIES, JSON.stringify(topActivities));
  },

  loadTopActivities: () => {
    const cachedTopActivities = localStorage.getItem(CACHE_KEY_TOP_ACTIVITIES);
    return cachedTopActivities ? JSON.parse(cachedTopActivities) : [];
  },

  clearCache: () => {
    localStorage.setItem(CACHE_KEY_ACTIVITIES, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_ACTIVITY_DETAILS, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_IMAGES, JSON.stringify({}));
    localStorage.setItem(CACHE_KEY_FACILITY_TYPES, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_ACTIVITY_TYPES, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_EDUCATORS, JSON.stringify([]));
    localStorage.setItem(CACHE_KEY_TOP_ACTIVITIES, JSON.stringify([]));
  }
};